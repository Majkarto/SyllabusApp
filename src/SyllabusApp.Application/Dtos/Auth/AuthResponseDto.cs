namespace SyllabusApp.Application.Dtos.Auth;

/// <summary>
/// Odpowiedź zwracana po pomyślnej rejestracji lub logowaniu.
/// Zawiera token JWT oraz podstawowe dane usera do wyświetlenia w UI
/// (bez konieczności robienia kolejnego requestu).
/// </summary>
public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }

    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public IList<string> Roles { get; set; } = new List<string>();
}
