namespace SyllabusApp.Domain.Entities;

public class Course : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Semester { get; set; }

    public int FieldOfStudyId { get; set; }
    public FieldOfStudy FieldOfStudy { get; set; } = null!;
    public ICollection<Syllabus> Syllabuses { get; set; } = new List<Syllabus>();
}