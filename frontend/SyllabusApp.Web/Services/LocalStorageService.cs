using System.Text.Json;
using Microsoft.JSInterop;
using SyllabusApp.Application.Dtos.Auth;

namespace SyllabusApp.Web.Services;

public class LocalStorageService
{
    private const string AuthStorageKey = "syllabus.auth";
    private readonly IJSRuntime _jsRuntime;

    public LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<AuthResponseDto?> GetAuthAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", AuthStorageKey);
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<AuthResponseDto>(json, ApiJson.Options);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public async Task SetAuthAsync(AuthResponseDto authData)
    {
        var json = JsonSerializer.Serialize(authData, ApiJson.Options);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AuthStorageKey, json);
    }

    public async Task ClearAuthAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", AuthStorageKey);
    }
}
