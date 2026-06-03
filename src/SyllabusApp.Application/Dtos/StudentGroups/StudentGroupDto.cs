namespace SyllabusApp.Application.Dtos.StudentGroups;

public class StudentGroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CurrentSemester { get; set; }
    public int StartYear { get; set; }
    public bool IsActive { get; set; } = true;
    public int FieldOfStudyId { get; set; }
    public string FieldOfStudyName { get; set; } = string.Empty;
}