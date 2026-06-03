using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.Literatures;

public class LiteratureDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? Publisher { get; set; }
    public int? PublicationYear { get; set; }
    public string? Isbn { get; set; }
    public LiteratureType Type { get; set; }
    public int SyllabusId { get; set; }
}
