using SyllabusApp.Application.Dtos.WorkloadItems;

namespace SyllabusApp.Application.Interfaces;

public interface IWorkloadItemService
{
    Task<IEnumerable<WorkloadItemDto>> GetAllAsync();
    Task<WorkloadItemDto?> GetByIdAsync(int id);
    Task<WorkloadItemResult> CreateAsync(CreateWorkloadItemDto dto);
    Task<WorkloadItemResult> UpdateAsync(int id, UpdateWorkloadItemDto dto);
    Task<bool> DeleteAsync(int id);
}

public class WorkloadItemResult
{
    public bool Success { get; init; }
    public WorkloadItemDto? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public WorkloadItemError Error { get; init; }

    public static WorkloadItemResult Ok(WorkloadItemDto data) => new() { Success = true, Data = data };
    public static WorkloadItemResult Fail(WorkloadItemError error, string message) =>
        new() { Success = false, Error = error, ErrorMessage = message };
}

public enum WorkloadItemError
{
    None,
    NotFound,
    SyllabusNotFound
}
