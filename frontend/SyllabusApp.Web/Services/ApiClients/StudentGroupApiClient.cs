using System.Net.Http.Json;
using SyllabusApp.Application.Dtos.StudentGroups;

namespace SyllabusApp.Web.Services.ApiClients;

public class StudentGroupApiClient
{
    private readonly HttpClient _http;

    public StudentGroupApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<StudentGroupDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<StudentGroupDto>>("student-groups", ApiJson.Options);
        return result ?? [];
    }

    public async Task<StudentGroupDto?> GetByIdAsync(int id)
    {
        var response = await _http.GetAsync($"student-groups/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<StudentGroupDto>(ApiJson.Options);
    }

    public async Task<ApiResult<StudentGroupDto>> CreateAsync(CreateStudentGroupDto dto)
    {
        var response = await _http.PostAsJsonAsync("student-groups", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<StudentGroupDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var created = await response.Content.ReadFromJsonAsync<StudentGroupDto>(ApiJson.Options);
        return ApiResult<StudentGroupDto>.Ok(created!);
    }

    public async Task<ApiResult<StudentGroupDto>> UpdateAsync(int id, UpdateStudentGroupDto dto)
    {
        var response = await _http.PutAsJsonAsync($"student-groups/{id}", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<StudentGroupDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var updated = await response.Content.ReadFromJsonAsync<StudentGroupDto>(ApiJson.Options);
        return ApiResult<StudentGroupDto>.Ok(updated!);
    }

    public async Task<ApiResult> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"student-groups/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        return ApiResult.Ok();
    }
}
