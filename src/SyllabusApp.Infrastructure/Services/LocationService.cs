using Microsoft.EntityFrameworkCore;
using SyllabusApp.Application.Dtos.Locations;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Infrastructure.Persistence;

namespace SyllabusApp.Infrastructure.Services;
public class LocationService : ILocationService
{
    private readonly SyllabusDbContext _dbContext;

    public LocationService(SyllabusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<LocationDto>> GetAllAsync()
    {
        return await _dbContext.Locations
            .AsNoTracking()
            .OrderBy(l => l.Name)
            .Select(l => l.ToDto())
            .ToListAsync();
    }

    public async Task<LocationDto?> GetByIdAsync(int id)
    {
        var location = await _dbContext.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);
        return location?.ToDto();
    }

    public async Task<LocationDto> CreateAsync(CreateLocationDto dto)
    {
        var location = dto.ToEntity();

        _dbContext.Locations.Add(location);
        await _dbContext.SaveChangesAsync();
        return location.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, UpdateLocationDto dto)
    {
        var location = await _dbContext.Locations.FirstOrDefaultAsync(l => l.Id == id);

        if (location == null)
        {
            return false;
        }

        location.ApplyUpdate(dto);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var location = await _dbContext.Locations.FirstOrDefaultAsync(l => l.Id == id);

        if (location == null)
        {
            return false;
        }

        _dbContext.Locations.Remove(location);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}