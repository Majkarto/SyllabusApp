using SyllabusApp.Application.Dtos.Faculties;

namespace SyllabusApp.Application.Interfaces;

public interface IFacultyService
{
    Task<IEnumerable<FacultyDto>> GetAllAsync();
    Task<FacultyDto?> GetByIdAsync(int id);
    Task<FacultyResult> CreateAsync(CreateFacultyDto dto);
    Task<FacultyResult> UpdateAsync(int id, UpdateFacultyDto dto);
    Task<bool> DeleteAsync(int id);
}

public class FacultyResult
{
    public bool Success { get; init; }
    public FacultyDto? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public FacultyError Error { get; init; }

    public static FacultyResult Ok(FacultyDto data) => new() { Success = true, Data = data };
    public static FacultyResult Fail(FacultyError error, string message) =>
        new() { Success = false, Error = error, ErrorMessage = message };
}

public enum FacultyError
{
    None,
    NotFound,
    LocationNotFound,
    CodeAlreadyExists
}