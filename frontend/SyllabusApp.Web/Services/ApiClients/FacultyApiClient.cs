using System.Net.Http.Json;
using SyllabusApp.Application.Dtos.Faculties;

namespace SyllabusApp.Web.Services.ApiClients;

public class FacultyApiClient
{
    private readonly HttpClient _http;

    public FacultyApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<FacultyDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<FacultyDto>>("faculties", ApiJson.Options);
        return result ?? [];
    }

    public async Task<FacultyDto?> GetByIdAsync(int id)
    {
        var response = await _http.GetAsync($"faculties/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<FacultyDto>(ApiJson.Options);
    }

    public async Task<ApiResult<FacultyDto>> CreateAsync(CreateFacultyDto dto)
    {
        var response = await _http.PostAsJsonAsync("faculties", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<FacultyDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var created = await response.Content.ReadFromJsonAsync<FacultyDto>(ApiJson.Options);
        return ApiResult<FacultyDto>.Ok(created!);
    }

    public async Task<ApiResult<FacultyDto>> UpdateAsync(int id, UpdateFacultyDto dto)
    {
        var response = await _http.PutAsJsonAsync($"faculties/{id}", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<FacultyDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var updated = await response.Content.ReadFromJsonAsync<FacultyDto>(ApiJson.Options);
        return ApiResult<FacultyDto>.Ok(updated!);
    }

    public async Task<ApiResult> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"faculties/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        return ApiResult.Ok();
    }
}
