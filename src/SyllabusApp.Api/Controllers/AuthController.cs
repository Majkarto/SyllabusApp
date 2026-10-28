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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return BadRequest(new { message = "Użytkownik o tym adresie email już istnieje." });
        }

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

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.Student);
        if (!roleResult.Succeeded)
        {
            return BadRequest(new
            {
                message = "Konto utworzone, ale nie udało się nadać roli. Czy role są wyseededowane?",
                errors = roleResult.Errors.Select(e => e.Description)
            });
        }

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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return Unauthorized(new { message = "Niepoprawny email lub hasło." });
        }

        if (!user.IsActive)
        {
            return Unauthorized(new { message = "Konto jest nieaktywne." });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            return Unauthorized(new { message = "Konto jest zablokowane z powodu zbyt wielu nieudanych prób. Spróbuj ponownie za 15 minut." });
        }

        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Niepoprawny email lub hasło." });
        }

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
    [HttpGet("me")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                    ?? User.FindFirst("email")?.Value;
        var firstName = User.FindFirst("firstName")?.Value;
        var lastName = User.FindFirst("lastName")?.Value;
        var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        return Ok(new
        {
            userId,
            email,
            firstName,
            lastName,
            roles
        });
    }
}
