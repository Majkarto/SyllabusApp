namespace SyllabusApp.Domain.Entities;

public class Literature : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? Publisher { get; set; }
    public int? PublicationYear { get; set; }
    public string? Isbn { get; set; }
    public LiteratureType Type { get; set; }
    public int SyllabusId { get; set; }
    public Syllabus Syllabus { get; set; } = null!;
}

public enum LiteratureType
{
    Required = 1,
    Supplementary = 2
}