using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Application.Interfaces;
public record TokenResult(string Token, DateTime ExpiresAt);

public interface ITokenService
{
    TokenResult GenerateAccessToken(User user, IList<string> roles);
}
