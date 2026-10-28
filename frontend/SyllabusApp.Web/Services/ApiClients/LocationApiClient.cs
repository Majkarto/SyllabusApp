using System.Net.Http.Json;
using SyllabusApp.Application.Dtos.Locations;

namespace SyllabusApp.Web.Services.ApiClients;

public class LocationApiClient
{
    private readonly HttpClient _http;

    public LocationApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<LocationDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<LocationDto>>("locations", ApiJson.Options);
        return result ?? [];
    }

    public async Task<LocationDto?> GetByIdAsync(int id)
    {
        var response = await _http.GetAsync($"locations/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<LocationDto>(ApiJson.Options);
    }

    public async Task<ApiResult<LocationDto>> CreateAsync(CreateLocationDto dto)
    {
        var response = await _http.PostAsJsonAsync("locations", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<LocationDto>.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        var created = await response.Content.ReadFromJsonAsync<LocationDto>(ApiJson.Options);
        return ApiResult<LocationDto>.Ok(created!);
    }

    public async Task<ApiResult> UpdateAsync(int id, UpdateLocationDto dto)
    {
        var response = await _http.PutAsJsonAsync($"locations/{id}", dto, ApiJson.Options);
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        return ApiResult.Ok();
    }

    public async Task<ApiResult> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"locations/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult.Fail(await ApiErrorHelper.ExtractErrorMessageAsync(response));
        }

        return ApiResult.Ok();
    }
}
