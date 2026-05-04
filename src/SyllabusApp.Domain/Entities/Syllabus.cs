namespace SyllabusApp.Domain.Entities;

public class Syllabus : BaseEntity
{
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
    public Course Course { get; set; } = null!;
    public int? LecturerId { get; set; }
    public User? Lecturer { get; set; }
    public int? CoordinatorId { get; set; }
    public User? Coordinator { get; set; }
    public ICollection<LearningOutcome> LearningOutcomes { get; set; } = new List<LearningOutcome>();
    public ICollection<Literature> Literature { get; set; } = new List<Literature>();
    public ICollection<WorkloadItem> WorkloadItems { get; set; } = new List<WorkloadItem>();
    public ICollection<SyllabusVersion> Versions { get; set; } = new List<SyllabusVersion>();
    public ICollection<SyllabusApproval> Approvals { get; set; } = new List<SyllabusApproval>();
}
public enum SyllabusStatus
{
    Draft = 1,
    UnderReview = 2,
    Approved = 3,
    Published = 4,
    Archived = 5
}