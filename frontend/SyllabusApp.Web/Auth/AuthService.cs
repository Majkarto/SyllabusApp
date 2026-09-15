using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using SyllabusApp.Application.Dtos.Auth;
using SyllabusApp.Web.Services;

namespace SyllabusApp.Web.Auth;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly LocalStorageService _localStorage;
    private readonly TokenAuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient http, LocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _localStorage = localStorage;
        _authStateProvider = (TokenAuthenticationStateProvider)authStateProvider;
    }

    public async Task<AuthOperationResult> LoginAsync(LoginDto dto)
    {
        var response = await _http.PostAsJsonAsync("auth/login", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return AuthOperationResult.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var authData = await response.Content.ReadFromJsonAsync<AuthResponseDto>(ApiJson.Options);
        await _localStorage.SetAuthAsync(authData!);
        _authStateProvider.NotifyUserAuthenticated(authData!);

        return AuthOperationResult.Ok();
    }

    public async Task<AuthOperationResult> RegisterAsync(RegisterDto dto)
    {
        var response = await _http.PostAsJsonAsync("auth/register", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return AuthOperationResult.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var authData = await response.Content.ReadFromJsonAsync<AuthResponseDto>(ApiJson.Options);
        await _localStorage.SetAuthAsync(authData!);
        _authStateProvider.NotifyUserAuthenticated(authData!);

        return AuthOperationResult.Ok();
    }

    public async Task LogoutAsync()
    {
        await _localStorage.ClearAuthAsync();
        _authStateProvider.NotifyUserLoggedOut();
    }
}
