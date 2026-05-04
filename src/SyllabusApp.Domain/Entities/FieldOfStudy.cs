namespace SyllabusApp.Domain.Entities;

public class FieldOfStudy : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public StudyLevel Level { get; set; }
    public int DurationSemesters { get; set; }
    public int FacultyId { get; set; }
    public Faculty Faculty { get; set; } = null!;
    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
}

public enum StudyLevel
{
    Bachelor = 1,
    Master = 2,
    Doctorate = 3
}