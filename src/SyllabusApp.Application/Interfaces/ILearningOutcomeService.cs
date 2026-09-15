using SyllabusApp.Application.Dtos.LearningOutcomes;

namespace SyllabusApp.Application.Interfaces;

public interface ILearningOutcomeService
{
    Task<IEnumerable<LearningOutcomeDto>> GetAllAsync();
    Task<LearningOutcomeDto?> GetByIdAsync(int id);
    Task<LearningOutcomeResult> CreateAsync(CreateLearningOutcomeDto dto);
    Task<LearningOutcomeResult> UpdateAsync(int id, UpdateLearningOutcomeDto dto);
    Task<bool> DeleteAsync(int id);
}

public class LearningOutcomeResult
{
    public bool Success { get; init; }
    public LearningOutcomeDto? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public LearningOutcomeError Error { get; init; }

    public static LearningOutcomeResult Ok(LearningOutcomeDto data) => new() { Success = true, Data = data };
    public static LearningOutcomeResult Fail(LearningOutcomeError error, string message) =>
        new() { Success = false, Error = error, ErrorMessage = message };
}

public enum LearningOutcomeError
{
    None,
    NotFound,
    SyllabusNotFound
}
