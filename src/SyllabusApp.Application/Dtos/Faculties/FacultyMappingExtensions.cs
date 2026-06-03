using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Application.Dtos.Faculties;

public static class FacultyMappingExtensions
{
    public static FacultyDto ToDto(this Faculty entity)
    {
        return new FacultyDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            LocationId = entity.LocationId,
            LocationName = entity.Location?.Name ?? string.Empty,
            LocationCity = entity.Location?.City ?? string.Empty,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }

    public static Faculty ToEntity(this CreateFacultyDto dto)
    {
        return new Faculty
        {
            Name = dto.Name,
            Code = dto.Code,
            LocationId = dto.LocationId,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public static void ApplyUpdate(this Faculty entity, UpdateFacultyDto dto)
    {
        entity.Name = dto.Name;
        entity.Code = dto.Code;
        entity.LocationId = dto.LocationId;
        entity.UpdatedAt = DateTime.UtcNow;
    }
}