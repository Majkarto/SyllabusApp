using System.ComponentModel.DataAnnotations;
namespace SyllabusApp.Application.Dtos.LearningOutcomes;
using SyllabusApp.Domain.Entities;
public class CreateLearningOutcomeDto
{

    [Required(ErrorMessage = "Kod jest wymagany.")]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Opis jest wymagany.")]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "wynik nauczania jest wymagany.")]
    public LearningOutcomeCategory Category { get; set; }
    
    [Required(ErrorMessage = "Id sylabusu jest wymagane.")]
    public int SyllabusId { get; set; }
}