namespace SyllabusApp.Domain.Entities;

public class Location : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string? BuildingCode { get; set; }

    public ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();
}