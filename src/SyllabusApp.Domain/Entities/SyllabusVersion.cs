namespace SyllabusApp.Domain.Entities;

public class SyllabusVersion : BaseEntity
{
    public int VersionNumber { get; set; }
    public string ChangeDescription { get; set; } = string.Empty;
    public string AcademicYear { get; set; } = string.Empty;
    public int EctsPoints { get; set; }
    public int LectureHours { get; set; }
    public int ExerciseHours { get; set; }
    public int LaboratoryHours { get; set; }
    public string CourseObjectives { get; set; } = string.Empty;
    public string AssessmentMethod { get; set; } = string.Empty;
    public string Prerequisites { get; set; } = string.Empty;
    public string SnapshotJson { get; set; } = string.Empty;
    public int SyllabusId { get; set; }
    public Syllabus Syllabus { get; set; } = null!;
    public int? CreatedById { get; set; }
    public User? CreatedBy { get; set; }
}