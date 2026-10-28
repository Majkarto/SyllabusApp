using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Application.Dtos.Syllabuses;

/// <summary>Kryteria wyszukiwania sylabusów (parametry query string).</summary>
public class SyllabusFilterDto
{
    /// <summary>Szukany tekst: nazwa/kod przedmiotu, opis, cele.</summary>
    public string? Search { get; set; }
    public string? AcademicYear { get; set; }
    public SyllabusStatus? Status { get; set; }
    public int? LocationId { get; set; }
    public int? FieldOfStudyId { get; set; }
    public int? Semester { get; set; }
}
