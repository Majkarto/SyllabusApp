using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Application.Dtos.Roles;

public static class RoleMappingExtensions
{
    public static RoleDto ToDto(this Role entity)
    {
        return new RoleDto()
        {
            Id = entity.Id,
            Name = entity.Name ?? string.Empty,
            Description = entity.Description,
        };
    }
    public static Role ToEntity(this CreateRoleDto dto)
    {
        return new Role
        {
            Name = dto.Name,
            Description = dto.Description,
        };
    }
    public static void ApplyUpdate(this Role entity, UpdateRoleDto dto)
    {
        entity.Name = dto.Name;
        entity.Description = dto.Description;
    }

}