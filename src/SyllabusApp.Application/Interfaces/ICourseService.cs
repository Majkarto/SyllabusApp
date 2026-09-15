using SyllabusApp.Application.Dtos.Courses;

namespace SyllabusApp.Application.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllAsync();
    Task<CourseDto?> GetByIdAsync(int id);
    Task<CourseResult> CreateAsync(CreateCourseDto dto);
    Task<CourseResult> UpdateAsync(int id, UpdateCourseDto dto);
    Task<bool> DeleteAsync(int id);
}

public class CourseResult
{
    public bool Success { get; init; }
    public CourseDto? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public CourseError Error { get; init; }

    public static CourseResult Ok(CourseDto data) => new() { Success = true, Data = data };
    public static CourseResult Fail(CourseError error, string message) =>
        new() { Success = false, Error = error, ErrorMessage = message };
}

public enum CourseError
{
    None,
    NotFound,
    FieldOfStudyNotFound,
    CodeAlreadyExists
}
