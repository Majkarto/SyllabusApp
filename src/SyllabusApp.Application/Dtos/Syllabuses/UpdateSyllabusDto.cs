using System.ComponentModel.DataAnnotations;

namespace SyllabusApp.Application.Dtos.Syllabuses;

public class UpdateSyllabusDto : SyllabusEditDto
{
    /// <summary>Krótki opis zmian - trafia do historii wersji.</summary>
    [MaxLength(500)]
    public string? ChangeDescription { get; set; }
}
