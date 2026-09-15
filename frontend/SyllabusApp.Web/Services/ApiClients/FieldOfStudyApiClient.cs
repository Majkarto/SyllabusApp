using System.Net.Http.Json;
using SyllabusApp.Application.Dtos.FieldOfStudies;

namespace SyllabusApp.Web.Services.ApiClients;

public class FieldOfStudyApiClient
{
    private readonly HttpClient _http;

    public FieldOfStudyApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<FieldOfStudyDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<FieldOfStudyDto>>("field-of-studies", ApiJson.Options);
        return result ?? [];
    }

    public async Task<FieldOfStudyDto?> GetByIdAsync(int id)
    {
        var response = await _http.GetAsync($"field-of-studies/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<FieldOfStudyDto>(ApiJson.Options);
    }

    public async Task<ApiResult<FieldOfStudyDto>> CreateAsync(CreateFieldOfStudyDto dto)
    {
        var response = await _http.PostAsJsonAsync("field-of-studies", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<FieldOfStudyDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var created = await response.Content.ReadFromJsonAsync<FieldOfStudyDto>(ApiJson.Options);
        return ApiResult<FieldOfStudyDto>.Ok(created!);
    }

    public async Task<ApiResult<FieldOfStudyDto>> UpdateAsync(int id, UpdateFieldOfStudyDto dto)
    {
        var response = await _http.PutAsJsonAsync($"field-of-studies/{id}", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<FieldOfStudyDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var updated = await response.Content.ReadFromJsonAsync<FieldOfStudyDto>(ApiJson.Options);
        return ApiResult<FieldOfStudyDto>.Ok(updated!);
    }

    public async Task<ApiResult> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"field-of-studies/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        return ApiResult.Ok();
    }
}
