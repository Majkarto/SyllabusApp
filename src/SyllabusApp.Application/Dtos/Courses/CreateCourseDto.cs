using System.ComponentModel.DataAnnotations;
namespace SyllabusApp.Application.Dtos.Courses;

public class CreateCourseDto
{
    [Required(ErrorMessage = "Nazwa jest wymagana.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kod jest wymagany.")]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Range(1, 12, ErrorMessage = "Semestr musi być z zakresu 1-12.")]
    public int Semester { get; set; }
    
    [Required(ErrorMessage = "Id kierunku studiów jest wymagane.")]
    public int FieldOfStudyId { get; set; }
}
