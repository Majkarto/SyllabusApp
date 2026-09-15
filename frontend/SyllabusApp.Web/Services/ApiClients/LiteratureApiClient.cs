using System.Net.Http.Json;
using SyllabusApp.Application.Dtos.Literatures;

namespace SyllabusApp.Web.Services.ApiClients;

public class LiteratureApiClient
{
    private readonly HttpClient _http;

    public LiteratureApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<LiteratureDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<LiteratureDto>>("literatures", ApiJson.Options);
        return result ?? [];
    }

    public async Task<ApiResult<LiteratureDto>> CreateAsync(CreateLiteratureDto dto)
    {
        var response = await _http.PostAsJsonAsync("literatures", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<LiteratureDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var created = await response.Content.ReadFromJsonAsync<LiteratureDto>(ApiJson.Options);
        return ApiResult<LiteratureDto>.Ok(created!);
    }

    public async Task<ApiResult> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"literatures/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        return ApiResult.Ok();
    }
}
