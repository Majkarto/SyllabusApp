using System.ComponentModel.DataAnnotations;
using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.Syllabuses;

public class UpdateSyllabusDto
{
    [Required(ErrorMessage = "Rok akademicki jest wymagany.")]
    public string AcademicYear { get; set; } = string.Empty;

    [Required(ErrorMessage = "Punkty ECTS  są wymagane.")]
    public int EctsPoints { get; set; }
    
    [Required(ErrorMessage = "Godziny wykładów są wymagane.")]
    public int LectureHours { get; set; }
    
    [Required(ErrorMessage = "Godziny ćwiczeń są wymagane.")]
    public int ExerciseHours { get; set; }
    
    [Required(ErrorMessage = "Godziny labolatoriów są wymagane.")]
    public int LaboratoryHours { get; set; }
    
    [Required(ErrorMessage = "Cel zajęć jest wymagany.")]
    [MaxLength(2000)]
    public string CourseObjectives { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Metoda oceny jest wymagana.")]
    [MaxLength(2000)]
    public string AssessmentMethod { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Warunki wstępne są wymagane.")]
    [MaxLength(2000)]
    public string Prerequisites { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Id kursu jest wymagane.")]
    public int CourseId { get; set; }
}