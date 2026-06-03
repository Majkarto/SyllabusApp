namespace SyllabusApp.Application.Dtos.Faculties;


public class FacultyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public int LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;   // z location
    public string LocationCity { get; set; } = string.Empty;   // z location

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}