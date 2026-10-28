using System.ComponentModel.DataAnnotations;
using SyllabusApp.Application.Validation;

namespace SyllabusApp.Application.Dtos.Syllabuses;

/// <summary>Wspólne pola formularza sylabusa (tworzenie i edycja).</summary>
public abstract class SyllabusEditDto : IValidatableObject
{
    [Required(ErrorMessage = "Rok akademicki jest wymagany.")]
    [RegularExpression(@"^\d{4}/\d{4}$", ErrorMessage = "Rok akademicki podaj w formacie RRRR/RRRR, np. 2026/2027.")]
    public string AcademicYear { get; set; } = string.Empty;

    [Required(ErrorMessage = "Punkty ECTS są wymagane.")]
    public int EctsPoints { get; set; }

    [Required(ErrorMessage = "Godziny wykładów są wymagane.")]
    public int LectureHours { get; set; }

    [Required(ErrorMessage = "Godziny ćwiczeń są wymagane.")]
    public int ExerciseHours { get; set; }

    [Required(ErrorMessage = "Godziny laboratoriów są wymagane.")]
    public int LaboratoryHours { get; set; }

    [MaxLength(4000)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cel zajęć jest wymagany.")]
    [MaxLength(2000)]
    public string CourseObjectives { get; set; } = string.Empty;

    [Required(ErrorMessage = "Metoda oceny jest wymagana.")]
    [MaxLength(2000)]
    public string AssessmentMethod { get; set; } = string.Empty;

    [Required(ErrorMessage = "Warunki wstępne są wymagane.")]
    [MaxLength(2000)]
    public string Prerequisites { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Wybierz przedmiot.")]
    public int CourseId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return SyllabusRules.ValidateHours(EctsPoints, LectureHours, ExerciseHours, LaboratoryHours)
            .Select(e => new ValidationResult(e, [nameof(EctsPoints)]));
    }
}
