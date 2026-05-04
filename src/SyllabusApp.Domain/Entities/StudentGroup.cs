namespace SyllabusApp.Domain.Entities;

public class StudentGroup : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int CurrentSemester { get; set; }
    public int StartYear { get; set; }
    public bool IsActive { get; set; } = true;
    public int FieldOfStudyId { get; set; }
    public FieldOfStudy FieldOfStudy { get; set; } = null!;
    public ICollection<User> Students { get; set; } = new List<User>();
}