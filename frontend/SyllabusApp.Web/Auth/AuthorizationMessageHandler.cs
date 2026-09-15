using System.Net.Http.Headers;
using SyllabusApp.Web.Services;

namespace SyllabusApp.Web.Auth;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly LocalStorageService _localStorage;

    public AuthorizationMessageHandler(LocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authData = await _localStorage.GetAuthAsync();
        if (authData != null && authData.ExpiresAt > DateTime.UtcNow)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authData.Token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
