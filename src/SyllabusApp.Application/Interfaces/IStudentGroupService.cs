using SyllabusApp.Application.Dtos.StudentGroups;

namespace SyllabusApp.Application.Interfaces;

public interface IStudentGroupService
{
    Task<IEnumerable<StudentGroupDto>> GetAllAsync();
    Task<StudentGroupDto?> GetByIdAsync(int id);
    Task<StudentGroupResult> CreateAsync(CreateStudentGroupDto dto);
    Task<StudentGroupResult> UpdateAsync(int id, UpdateStudentGroupDto dto);
    Task<bool> DeleteAsync(int id);
}

public class StudentGroupResult
{
    public bool Success { get; init; }
    public StudentGroupDto? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public StudentGroupError Error { get; init; }

    public static StudentGroupResult Ok(StudentGroupDto data) => new() { Success = true, Data = data };
    public static StudentGroupResult Fail(StudentGroupError error, string message) =>
        new() { Success = false, Error = error, ErrorMessage = message };
}

public enum StudentGroupError
{
    None,
    NotFound,
    FieldOfStudyNotFound,
    NameAlreadyExists
}
