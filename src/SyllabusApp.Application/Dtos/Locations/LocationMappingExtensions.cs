using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Application.Dtos.Locations;
public static class LocationMappingExtensions
{
    public static LocationDto ToDto(this Location entity)
    {
        return new LocationDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address,
            City = entity.City,
            PostalCode = entity.PostalCode,
            BuildingCode = entity.BuildingCode,
            CreatedAt = entity.CreatedAt,
        };
    }
    public static Location ToEntity(this CreateLocationDto dto)
    {
        return new Location
        {
            Name = dto.Name,
            Address = dto.Address,
            City = dto.City,
            PostalCode = dto.PostalCode,
            BuildingCode = dto.BuildingCode,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
    }
    public static void ApplyUpdate(this Location entity, UpdateLocationDto dto)
    {
        entity.Name = dto.Name;
        entity.Address = dto.Address;
        entity.City = dto.City;
        entity.PostalCode = dto.PostalCode;
        entity.BuildingCode = dto.BuildingCode;
        entity.UpdatedAt = DateTime.UtcNow;
    }
}