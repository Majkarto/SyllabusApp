using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Application.Dtos.Syllabuses;

public class SyllabusFilterDto
{
    public string? Search { get; set; }
    public string? AcademicYear { get; set; }
    public SyllabusStatus? Status { get; set; }
    public int? LocationId { get; set; }
    public int? FieldOfStudyId { get; set; }
    public int? Semester { get; set; }
}
