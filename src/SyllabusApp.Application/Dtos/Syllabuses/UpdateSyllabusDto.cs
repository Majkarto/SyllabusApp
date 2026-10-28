using System.ComponentModel.DataAnnotations;

namespace SyllabusApp.Application.Dtos.Syllabuses;

public class UpdateSyllabusDto : SyllabusEditDto
{
    [MaxLength(500)]
    public string? ChangeDescription { get; set; }
}
