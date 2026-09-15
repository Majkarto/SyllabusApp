using System.Net.Http.Json;
using SyllabusApp.Application.Dtos.Syllabuses;

namespace SyllabusApp.Web.Services.ApiClients;

public class SyllabusApiClient
{
    private readonly HttpClient _http;

    public SyllabusApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<SyllabusDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<SyllabusDto>>("syllabuses", ApiJson.Options);
        return result ?? [];
    }

    public async Task<SyllabusDto?> GetByIdAsync(int id)
    {
        var response = await _http.GetAsync($"syllabuses/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<SyllabusDto>(ApiJson.Options);
    }

    public async Task<ApiResult<SyllabusDto>> CreateAsync(CreateSyllabusDto dto)
    {
        var response = await _http.PostAsJsonAsync("syllabuses", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<SyllabusDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var created = await response.Content.ReadFromJsonAsync<SyllabusDto>(ApiJson.Options);
        return ApiResult<SyllabusDto>.Ok(created!);
    }

    public async Task<ApiResult<SyllabusDto>> UpdateAsync(int id, UpdateSyllabusDto dto)
    {
        var response = await _http.PutAsJsonAsync($"syllabuses/{id}", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<SyllabusDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var updated = await response.Content.ReadFromJsonAsync<SyllabusDto>(ApiJson.Options);
        return ApiResult<SyllabusDto>.Ok(updated!);
    }

    public async Task<ApiResult> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"syllabuses/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        return ApiResult.Ok();
    }
}
