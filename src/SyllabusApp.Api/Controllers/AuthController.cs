using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SyllabusApp.Application.Dtos.Auth;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Domain.Constants;
using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Rejestracja nowego użytkownika. Domyślnie tworzy konto z rolą Student.
    /// Wyższe role (Lecturer, Dean, Admin) nadaje admin osobnym endpointem.
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        // 1. Sprawdzamy walidację z atrybutów DTO ([Required], [EmailAddress] itp.)
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // 2. Sprawdzamy czy user o tym mailu już istnieje
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return BadRequest(new { message = "Użytkownik o tym adresie email już istnieje." });
        }

        // 3. Tworzymy nowego usera
        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        // UserManager.CreateAsync ZAPISUJE do bazy + hashuje hasło
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            // Identity zwraca własną listę błędów (np. "hasło za słabe")
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        // 4. Nadajemy rolę Student
        // UWAGA: jeśli rola Student nie istnieje w bazie (a w tym momencie nie istnieje, 
        // bo seeder będzie dopiero w Cz.5), to AddToRoleAsync rzuci błędem.
        // To jest spodziewane na ten moment - zobaczysz "Role 'Student' does not exist".
        var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.Student);
        if (!roleResult.Succeeded)
        {
            return BadRequest(new
            {
                message = "Konto utworzone, ale nie udało się nadać roli. Czy role są wyseededowane?",
                errors = roleResult.Errors.Select(e => e.Description)
            });
        }

        // 5. Generujemy token i zwracamy odpowiedź
        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateAccessToken(user, roles);

        var response = new AuthResponseDto
        {
            Token = token.Token,
            ExpiresAt = token.ExpiresAt,
            UserId = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles,
        };

        return Ok(response);
    }

    /// <summary>
    /// Logowanie - sprawdza email i hasło, zwraca token JWT.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        // 1. Walidacja DTO
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // 2. Szukamy usera po emailu
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            // Zwracamy ogólny komunikat - nie zdradzamy czy email istnieje
            // (zapobiega "user enumeration attack" - sprawdzaniu jacy userzy istnieją)
            return Unauthorized(new { message = "Niepoprawny email lub hasło." });
        }

        // 3. Sprawdzamy czy konto aktywne (soft-delete pattern)
        if (!user.IsActive)
        {
            return Unauthorized(new { message = "Konto jest nieaktywne." });
        }

        // 4. Sprawdzamy hasło
        // CheckPasswordSignInAsync (NIE PasswordSignInAsync) - bo nie chcemy tworzyć cookie,
        // tylko zwalidować hasło. lockoutOnFailure: true włącza mechanizm blokady po 5 nieudanych próbach
        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            return Unauthorized(new { message = "Konto jest zablokowane z powodu zbyt wielu nieudanych prób. Spróbuj ponownie za 15 minut." });
        }

        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Niepoprawny email lub hasło." });
        }

        // 5. Hasło OK - generujemy token
        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateAccessToken(user, roles);

        var response = new AuthResponseDto
        {
            Token = token.Token,
            ExpiresAt = token.ExpiresAt,
            UserId = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles,
        };

        return Ok(response);
    }
}
