using System.ComponentModel.DataAnnotations;

namespace SyllabusApp.Application.Dtos.Auth;

/// <summary>
/// Dane wymagane do zalogowania.
/// </summary>
public class LoginDto
{
    [Required(ErrorMessage = "Email jest wymagany.")]
    [EmailAddress(ErrorMessage = "Niepoprawny format adresu email.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hasło jest wymagane.")]
    public string Password { get; set; } = string.Empty;
}
