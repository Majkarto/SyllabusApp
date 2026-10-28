using System.Net.Http.Json;
using SyllabusApp.Application.Dtos.Curriculum;
using SyllabusApp.Application.Dtos.Syllabuses;
using SyllabusApp.Application.Dtos.SyllabusVersions;

namespace SyllabusApp.Web.Services.ApiClients;

public class SyllabusApiClient
{
    private readonly HttpClient _http;

    public SyllabusApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<SyllabusDto>> GetAllAsync(SyllabusFilterDto? filter = null)
    {
        var query = new List<string>();
        if (filter != null)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search)) query.Add($"search={Uri.EscapeDataString(filter.Search)}");
            if (!string.IsNullOrWhiteSpace(filter.AcademicYear)) query.Add($"academicYear={Uri.EscapeDataString(filter.AcademicYear)}");
            if (filter.Status.HasValue) query.Add($"status={filter.Status}");
            if (filter.LocationId.HasValue) query.Add($"locationId={filter.LocationId}");
            if (filter.FieldOfStudyId.HasValue) query.Add($"fieldOfStudyId={filter.FieldOfStudyId}");
            if (filter.Semester.HasValue) query.Add($"semester={filter.Semester}");
        }

        var url = query.Count == 0 ? "syllabuses" : "syllabuses?" + string.Join("&", query);
        var result = await _http.GetFromJsonAsync<List<SyllabusDto>>(url, ApiJson.Options);
        return result ?? [];
    }

    public async Task<List<SyllabusVersionDto>> GetVersionsAsync(int id)
    {
        var result = await _http.GetFromJsonAsync<List<SyllabusVersionDto>>($"syllabuses/{id}/versions", ApiJson.Options);
        return result ?? [];
    }

    public async Task<CurriculumGridDto?> GetCurriculumGridAsync(int fieldOfStudyId, string? academicYear)
    {
        var url = $"syllabuses/curriculum-grid?fieldOfStudyId={fieldOfStudyId}";
        if (!string.IsNullOrWhiteSpace(academicYear)) url += $"&academicYear={Uri.EscapeDataString(academicYear)}";
        return await _http.GetFromJsonAsync<CurriculumGridDto>(url, ApiJson.Options);
    }

    public async Task<ApiResult<byte[]>> ExportAsync(int id, string format)
    {
        var response = await _http.GetAsync($"syllabuses/{id}/export/{format}");
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<byte[]>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }
        return ApiResult<byte[]>.Ok(await response.Content.ReadAsByteArrayAsync());
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
