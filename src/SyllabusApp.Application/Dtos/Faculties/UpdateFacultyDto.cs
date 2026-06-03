using System.ComponentModel.DataAnnotations;

namespace SyllabusApp.Application.Dtos.Faculties;

public class UpdateFacultyDto
{
    [Required(ErrorMessage = "Nazwa jest wymagana.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kod jest wymagany.")]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Id lokalizacji jest wymagane.")]
    public int LocationId { get; set; }
}
