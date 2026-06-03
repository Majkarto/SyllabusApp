using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.Syllabuses;

public class SyllabusDto
{
    public int Id { get; set; }
    public string AcademicYear { get; set; } = string.Empty;
    public int EctsPoints { get; set; }
    public int LectureHours { get; set; }
    public int ExerciseHours { get; set; }
    public int LaboratoryHours { get; set; }
    public string CourseObjectives { get; set; } = string.Empty;
    public string AssessmentMethod { get; set; } = string.Empty;
    public string Prerequisites { get; set; } = string.Empty;
    public SyllabusStatus Status { get; set; } = SyllabusStatus.Draft;
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
}
