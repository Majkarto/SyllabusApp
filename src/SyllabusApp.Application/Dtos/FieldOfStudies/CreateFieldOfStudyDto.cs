using System.ComponentModel.DataAnnotations;
using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.FieldOfStudies;

public class CreateFieldOfStudyDto
{
    [Required(ErrorMessage = "Nazwa jest wymagana.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kod jest wymagany.")]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Poziom nauczania jest wymagany.")]
    public StudyLevel Level { get; set; }
    
    [Required(ErrorMessage = "Liczba semestrów jest wymagana.")]
    [Range(1, 20, ErrorMessage = "Liczba semestrów musi być z zakresu 1-20.")]
    public int DurationSemesters { get; set; }
    
    [Required(ErrorMessage = "Id placówki jest wymagane.")]
    public int FacultyId { get; set; }
}