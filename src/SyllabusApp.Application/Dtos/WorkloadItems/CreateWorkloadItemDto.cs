using System.ComponentModel.DataAnnotations;
using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.WorkloadItems;

public class CreateWorkloadItemDto
{
    [Required(ErrorMessage = "Opis jest wymagany.")]
    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Godziny są wymagana.")]
    public int Hours { get; set; }
    
    [Required(ErrorMessage = "Typ zadania jest wymagany.")]
    public WorkloadType Type { get; set; }
    
    [Required(ErrorMessage = "Id sylabusa jest wymagane.")]
    public int SyllabusId { get; set; }
}