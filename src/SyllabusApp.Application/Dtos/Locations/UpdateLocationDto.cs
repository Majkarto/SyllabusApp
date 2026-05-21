using System.ComponentModel.DataAnnotations;

namespace SyllabusApp.Application.Dtos.Locations;
public class UpdateLocationDto
{
    [Required(ErrorMessage = "Nazwa jest wymagana.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adres jest wymagany.")]
    [MaxLength(300)]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Miasto jest wymagane.")]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kod pocztowy jest wymagany.")]
    [MaxLength(10)]
    public string PostalCode { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? BuildingCode { get; set; }
}