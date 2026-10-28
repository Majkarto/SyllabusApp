using SyllabusApp.Application.Dtos.Curriculum;
using SyllabusApp.Application.Dtos.Syllabuses;
using SyllabusApp.Application.Dtos.SyllabusVersions;

namespace SyllabusApp.Application.Interfaces;

public interface ISyllabusService
{
    Task<IEnumerable<SyllabusDto>> GetAllAsync(SyllabusFilterDto? filter = null);
    Task<SyllabusDto?> GetByIdAsync(int id);
    Task<SyllabusResult> CreateAsync(CreateSyllabusDto dto, int lecturerId);
    Task<SyllabusResult> UpdateAsync(int id, UpdateSyllabusDto dto, int? userId = null);
    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<SyllabusVersionDto>?> GetVersionsAsync(int syllabusId);

    Task<CurriculumGridDto?> GetCurriculumGridAsync(int fieldOfStudyId, string? academicYear);
}

public class SyllabusResult
{
    public bool Success { get; init; }
    public SyllabusDto? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public SyllabusError Error { get; init; }

    public static SyllabusResult Ok(SyllabusDto data) => new() { Success = true, Data = data };
    public static SyllabusResult Fail(SyllabusError error, string message) =>
        new() { Success = false, Error = error, ErrorMessage = message };
}

public enum SyllabusError
{
    None,
    NotFound,
    CourseNotFound
}
