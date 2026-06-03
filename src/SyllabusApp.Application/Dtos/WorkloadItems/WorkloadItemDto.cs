using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.WorkloadItems;

public class WorkloadItemDto
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Hours { get; set; }
    public WorkloadType Type { get; set; }
    public int SyllabusId { get; set; }
    public string SyllabusAcademicYear { get; set; } = string.Empty;
}