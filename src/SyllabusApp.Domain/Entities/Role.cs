using Microsoft.AspNetCore.Identity;

namespace SyllabusApp.Domain.Entities;

public class Role : IdentityRole<int>
{
    public string? Description { get; set; }
}