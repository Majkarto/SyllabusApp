using System.Net.Http.Json;

namespace SyllabusApp.Web.Services;

public static class ApiErrorHelper
{
    public static async Task<string> ExtractErrorMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var payload = await response.Content.ReadFromJsonAsync<ApiErrorPayload>(ApiJson.Options);
            if (!string.IsNullOrWhiteSpace(payload?.Message))
            {
                return payload.Message;
            }

            if (payload?.Errors is { Count: > 0 })
            {
                return string.Join(" ", payload.Errors);
            }
        }
        catch (Exception)
        {
        }

        return $"Żądanie nie powiodło się (kod {(int)response.StatusCode}).";
    }

    private class ApiErrorPayload
    {
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
