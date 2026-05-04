namespace SyllabusApp.Domain.Entities;

public class SyllabusApproval : BaseEntity
{
    public ApprovalAction Action { get; set; }
    public string? Comment { get; set; }
    public DateTime ActionDate { get; set; } = DateTime.UtcNow;
    public int SyllabusId { get; set; }
    public Syllabus Syllabus { get; set; } = null!;
    public int? SyllabusVersionId { get; set; }
    public SyllabusVersion? SyllabusVersion { get; set; }
    public int? PerformedById { get; set; }
    public User? PerformedBy { get; set; }
}
public enum ApprovalAction
{
    Submitted = 1,
    Approved = 2,
    Rejected = 3,
    Published = 4,
    Withdrawn = 5,
    Archived = 6
}