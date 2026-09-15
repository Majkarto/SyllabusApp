using SyllabusApp.Application.Dtos.Literatures;

namespace SyllabusApp.Application.Interfaces;

public interface ILiteratureService
{
    Task<IEnumerable<LiteratureDto>> GetAllAsync();
    Task<LiteratureDto?> GetByIdAsync(int id);
    Task<LiteratureResult> CreateAsync(CreateLiteratureDto dto);
    Task<LiteratureResult> UpdateAsync(int id, UpdateLiteratureDto dto);
    Task<bool> DeleteAsync(int id);
}

public class LiteratureResult
{
    public bool Success { get; init; }
    public LiteratureDto? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public LiteratureError Error { get; init; }

    public static LiteratureResult Ok(LiteratureDto data) => new() { Success = true, Data = data };
    public static LiteratureResult Fail(LiteratureError error, string message) =>
        new() { Success = false, Error = error, ErrorMessage = message };
}

public enum LiteratureError
{
    None,
    NotFound,
    SyllabusNotFound
}
