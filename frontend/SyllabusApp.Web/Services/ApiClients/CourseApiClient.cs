using System.Net.Http.Json;
using SyllabusApp.Application.Dtos.Courses;

namespace SyllabusApp.Web.Services.ApiClients;

public class CourseApiClient
{
    private readonly HttpClient _http;

    public CourseApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<CourseDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<CourseDto>>("courses", ApiJson.Options);
        return result ?? [];
    }

    public async Task<CourseDto?> GetByIdAsync(int id)
    {
        var response = await _http.GetAsync($"courses/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<CourseDto>(ApiJson.Options);
    }

    public async Task<ApiResult<CourseDto>> CreateAsync(CreateCourseDto dto)
    {
        var response = await _http.PostAsJsonAsync("courses", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<CourseDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var created = await response.Content.ReadFromJsonAsync<CourseDto>(ApiJson.Options);
        return ApiResult<CourseDto>.Ok(created!);
    }

    public async Task<ApiResult<CourseDto>> UpdateAsync(int id, UpdateCourseDto dto)
    {
        var response = await _http.PutAsJsonAsync($"courses/{id}", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<CourseDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var updated = await response.Content.ReadFromJsonAsync<CourseDto>(ApiJson.Options);
        return ApiResult<CourseDto>.Ok(updated!);
    }

    public async Task<ApiResult> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"courses/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        return ApiResult.Ok();
    }
}
