namespace SyllabusApp.Domain.Entities;

public class WorkloadItem : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public int Hours { get; set; }
    public WorkloadType Type { get; set; }
    public int SyllabusId { get; set; }
    public Syllabus Syllabus { get; set; } = null!;
}

public enum WorkloadType
{
    Contact = 1,
    Independent = 2,
    Assessment = 3
}