using SyllabusApp.Application.Dtos.FieldOfStudies;

namespace SyllabusApp.Application.Interfaces;

public interface IFieldOfStudyService
{
    Task<IEnumerable<FieldOfStudyDto>> GetAllAsync();
    Task<FieldOfStudyDto?> GetByIdAsync(int id);
    Task<FieldOfStudyResult> CreateAsync(CreateFieldOfStudyDto dto);
    Task<FieldOfStudyResult> UpdateAsync(int id, UpdateFieldOfStudyDto dto);
    Task<bool> DeleteAsync(int id);
}

public class FieldOfStudyResult
{
    public bool Success { get; init; }
    public FieldOfStudyDto? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public FieldOfStudyError Error { get; init; }

    public static FieldOfStudyResult Ok(FieldOfStudyDto data) => new() { Success = true, Data = data };
    public static FieldOfStudyResult Fail(FieldOfStudyError error, string message) =>
        new() { Success = false, Error = error, ErrorMessage = message };
}

public enum FieldOfStudyError
{
    None,
    NotFound,
    FacultyNotFound,
    CodeAlreadyExists
}
