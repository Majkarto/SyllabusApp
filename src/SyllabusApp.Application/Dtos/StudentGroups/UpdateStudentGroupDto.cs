using System.ComponentModel.DataAnnotations;
namespace SyllabusApp.Application.Dtos.StudentGroups;

public class UpdateStudentGroupDto
{
    [Required(ErrorMessage = "Imię jest wymagane.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Semestr jest wymagany.")]
    public int CurrentSemester { get; set; }
    
    [Required(ErrorMessage = "Rok zaczęcia jest wymagany.")]
    public int StartYear { get; set; }
    
    [Required(ErrorMessage = "Kierunek nauki jest wymagany.")]
    public int FieldOfStudyId { get; set; }
}