using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Application.Interfaces;

public interface ITokenService
{
    /// <summary>
    /// Generuje JWT access token dla podanego użytkownika i jego ról.
    /// </summary>
    string GenerateAccessToken(User user, IList<string> roles);
}
