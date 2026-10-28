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
    public string Description { get; set; } = string.Empty;
    public string CourseObjectives { get; set; } = string.Empty;
    public string AssessmentMethod { get; set; } = string.Empty;
    public string Prerequisites { get; set; } = string.Empty;
    public SyllabusStatus Status { get; set; } = SyllabusStatus.Draft;
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public int Semester { get; set; }
    public int? FieldOfStudyId { get; set; }
    public string FieldOfStudyName { get; set; } = string.Empty;
    public int? LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public int CurrentVersion { get; set; }
}
