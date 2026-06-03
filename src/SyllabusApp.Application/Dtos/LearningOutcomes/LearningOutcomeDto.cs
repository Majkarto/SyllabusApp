using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.LearningOutcomes;

public class LearningOutcomeDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public LearningOutcomeCategory Category { get; set; }
    public int SyllabusId { get; set; }
    public string SyllabusAcademicYear { get; set; } = string.Empty;
}