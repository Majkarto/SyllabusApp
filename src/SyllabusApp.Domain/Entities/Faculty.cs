namespace SyllabusApp.Domain.Entities;

public class Faculty : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;
    public int? DeanId { get; set; }
    public User? Dean { get; set; }

    public ICollection<FieldOfStudy> FieldsOfStudy { get; set; } = new List<FieldOfStudy>();
    public ICollection<User> Employees { get; set; } = new List<User>();
}