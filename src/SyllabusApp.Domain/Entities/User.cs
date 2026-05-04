using Microsoft.AspNetCore.Identity;

namespace SyllabusApp.Domain.Entities;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AcademicTitle { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public int? FacultyId { get; set; }
    public Faculty? Faculty { get; set; }

    public ICollection<Syllabus> SyllabusesAsLecturer { get; set; } = new List<Syllabus>();

    public ICollection<Syllabus> SyllabusesAsCoordinator { get; set; } = new List<Syllabus>();

    public string FullName => string.IsNullOrEmpty(AcademicTitle)
        ? $"{FirstName} {LastName}"
        : $"{AcademicTitle} {FirstName} {LastName}";
}