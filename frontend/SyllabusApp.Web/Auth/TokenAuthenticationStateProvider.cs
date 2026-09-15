using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using SyllabusApp.Application.Dtos.Auth;
using SyllabusApp.Web.Services;

namespace SyllabusApp.Web.Auth;

public class TokenAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal AnonymousPrincipal = new(new ClaimsIdentity());

    private readonly LocalStorageService _localStorage;

    public AuthResponseDto? CurrentUser { get; private set; }

    public TokenAuthenticationStateProvider(LocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authData = await _localStorage.GetAuthAsync();
        if (authData == null || authData.ExpiresAt <= DateTime.UtcNow)
        {
            CurrentUser = null;
            return new AuthenticationState(AnonymousPrincipal);
        }

        CurrentUser = authData;
        return new AuthenticationState(new ClaimsPrincipal(BuildIdentity(authData)));
    }

    public void NotifyUserAuthenticated(AuthResponseDto authData)
    {
        CurrentUser = authData;
        var principal = new ClaimsPrincipal(BuildIdentity(authData));
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public void NotifyUserLoggedOut()
    {
        CurrentUser = null;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(AnonymousPrincipal)));
    }

    private static ClaimsIdentity BuildIdentity(AuthResponseDto authData)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, authData.UserId.ToString()),
            new(ClaimTypes.Email, authData.Email),
            new("firstName", authData.FirstName),
            new("lastName", authData.LastName),
        };
        claims.AddRange(authData.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return new ClaimsIdentity(claims, authenticationType: "jwt");
    }
}
