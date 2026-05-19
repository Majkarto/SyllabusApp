using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Application.Interfaces;

/// <summary>
/// Wynik generowania tokenu - sam token + kiedy wygasa.
/// Data wygaśnięcia jest potrzebna klientowi, żeby wiedział kiedy go odświeżyć.
/// </summary>
public record TokenResult(string Token, DateTime ExpiresAt);

public interface ITokenService
{
    /// <summary>
    /// Generuje JWT access token dla podanego użytkownika i jego ról.
    /// </summary>
    TokenResult GenerateAccessToken(User user, IList<string> roles);
}
