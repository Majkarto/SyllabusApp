using System.ComponentModel.DataAnnotations;

namespace SyllabusApp.Application.Dtos.Auth;

/// <summary>
/// Dane wymagane do rejestracji nowego użytkownika.
/// Wszyscy rejestrowani userzy otrzymują rolę Student.
/// Inne role nadaje admin osobnym endpointem (Etap 5).
/// </summary>
public class RegisterDto
{
    [Required(ErrorMessage = "Email jest wymagany.")]
    [EmailAddress(ErrorMessage = "Niepoprawny format adresu email.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hasło jest wymagane.")]
    [MinLength(8, ErrorMessage = "Hasło musi mieć co najmniej 8 znaków.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Imię jest wymagane.")]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nazwisko jest wymagane.")]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
}
