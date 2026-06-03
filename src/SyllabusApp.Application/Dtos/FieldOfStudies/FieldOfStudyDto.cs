using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.FieldOfStudies;

public class FieldOfStudyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public StudyLevel Level { get; set; }
    public int DurationSemesters { get; set; }
    public int FacultyId { get; set; }
    public string FacultyName { get; set; } = string.Empty;   // z Faculty
    public string FacultyCode { get; set; } = string.Empty;   // z Faculty

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
