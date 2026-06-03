using System.ComponentModel.DataAnnotations;
namespace SyllabusApp.Application.Dtos.Roles;

public class CreateRoleDto
{
    [Required(ErrorMessage = "Nazwa jest wymagana.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}