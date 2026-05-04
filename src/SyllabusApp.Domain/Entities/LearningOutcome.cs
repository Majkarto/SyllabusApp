namespace SyllabusApp.Domain.Entities;

public class LearningOutcome : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public LearningOutcomeCategory Category { get; set; }
    public int SyllabusId { get; set; }
    public Syllabus Syllabus { get; set; } = null!;
}

public enum LearningOutcomeCategory
{
    Knowledge = 1,
    Skills = 2,
    Social = 3
}