using SyllabusApp.Application.Dtos.Locations;

namespace SyllabusApp.Application.Interfaces;

public interface ILocationService
{
    Task<IEnumerable<LocationDto>> GetAllAsync();
    Task<LocationDto?> GetByIdAsync(int id);
    Task<LocationDto> CreateAsync(CreateLocationDto dto);
    Task<bool> UpdateAsync(int id, UpdateLocationDto dto);
    Task<bool> DeleteAsync(int id);
}