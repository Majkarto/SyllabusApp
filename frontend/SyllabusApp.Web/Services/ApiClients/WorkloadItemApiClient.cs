using System.Net.Http.Json;
using SyllabusApp.Application.Dtos.WorkloadItems;

namespace SyllabusApp.Web.Services.ApiClients;

public class WorkloadItemApiClient
{
    private readonly HttpClient _http;

    public WorkloadItemApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<WorkloadItemDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<WorkloadItemDto>>("workload-items", ApiJson.Options);
        return result ?? [];
    }

    public async Task<ApiResult<WorkloadItemDto>> CreateAsync(CreateWorkloadItemDto dto)
    {
        var response = await _http.PostAsJsonAsync("workload-items", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<WorkloadItemDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var created = await response.Content.ReadFromJsonAsync<WorkloadItemDto>(ApiJson.Options);
        return ApiResult<WorkloadItemDto>.Ok(created!);
    }

    public async Task<ApiResult> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"workload-items/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        return ApiResult.Ok();
    }
}
