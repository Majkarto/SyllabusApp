using System.Net.Http.Json;
using SyllabusApp.Application.Dtos.LearningOutcomes;

namespace SyllabusApp.Web.Services.ApiClients;

public class LearningOutcomeApiClient
{
    private readonly HttpClient _http;

    public LearningOutcomeApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<LearningOutcomeDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<LearningOutcomeDto>>("learning-outcomes", ApiJson.Options);
        return result ?? [];
    }

    public async Task<ApiResult<LearningOutcomeDto>> CreateAsync(CreateLearningOutcomeDto dto)
    {
        var response = await _http.PostAsJsonAsync("learning-outcomes", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<LearningOutcomeDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var created = await response.Content.ReadFromJsonAsync<LearningOutcomeDto>(ApiJson.Options);
        return ApiResult<LearningOutcomeDto>.Ok(created!);
    }

    public async Task<ApiResult> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"learning-outcomes/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        return ApiResult.Ok();
    }
}
