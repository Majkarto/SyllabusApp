using System.ComponentModel.DataAnnotations;
using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Application.Dtos.Literatures;

public class UpdateLiteratureDto
{
    [Required(ErrorMessage = "Tytuł jest wymagany.")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Autor jest wymagany.")]
    [MaxLength(50)]
    public string Author { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Publisher { get; set; }
    
    public int? PublicationYear { get; set; }

    [MaxLength(20)]
    public string? Isbn { get; set; }

    [Required(ErrorMessage = "Rodzaj literatury jest wymagany.")]
    public LiteratureType Type { get; set; }
    
    [Required(ErrorMessage = "Id sylabusa jest wymagane.")]
    public int SyllabusId { get; set; }
}