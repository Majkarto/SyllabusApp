namespace SyllabusApp.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AcademicTitle { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public int? FacultyId { get; set; }
    public Faculty? Faculty { get; set; }
    public ICollection<Syllabus> SyllabusesAsLecturer { get; set; } = new List<Syllabus>();
    public ICollection<Syllabus> SyllabusesAsCoordinator { get; set; } = new List<Syllabus>();
    public string FullName => string.IsNullOrEmpty(AcademicTitle)
        ? $"{FirstName} {LastName}"
        : $"{AcademicTitle} {FirstName} {LastName}";
}

public enum UserRole
{
    Student = 1,
    Lecturer = 2,
    Dean = 3,
    Admin = 4
}