using Microsoft.EntityFrameworkCore;
using SyllabusApp.Application.Dtos.Faculties;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Infrastructure.Persistence;

namespace SyllabusApp.Infrastructure.Services;

public class FacultyService : IFacultyService
{
    private readonly SyllabusDbContext _dbContext;

    public FacultyService(SyllabusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<FacultyDto>> GetAllAsync()
    {
        return await _dbContext.Faculties
            .AsNoTracking()
            .Include(f => f.Location)
            .OrderBy(f => f.Name)
            .Select(f => f.ToDto())
            .ToListAsync();
    }

    public async Task<FacultyDto?> GetByIdAsync(int id)
    {
        var faculty = await _dbContext.Faculties
            .AsNoTracking()
            .Include(f => f.Location)
            .FirstOrDefaultAsync(f => f.Id == id);

        return faculty?.ToDto();
    }

    public async Task<FacultyResult> CreateAsync(CreateFacultyDto dto)
    {
        var locationExists = await _dbContext.Locations.AnyAsync(l => l.Id == dto.LocationId);
        if (!locationExists)
        {
            return FacultyResult.Fail(FacultyError.LocationNotFound,
                $"Nie znaleziono lokalizacji o id {dto.LocationId}.");
        }
        
        var codeExists = await _dbContext.Faculties.AnyAsync(f => f.Code == dto.Code);
        if (codeExists)
        {
            return FacultyResult.Fail(FacultyError.CodeAlreadyExists,
                $"Wydział o kodzie '{dto.Code}' już istnieje.");
        }

        var faculty = dto.ToEntity();
        _dbContext.Faculties.Add(faculty);
        await _dbContext.SaveChangesAsync();
        
        await _dbContext.Entry(faculty).Reference(f => f.Location).LoadAsync();

        return FacultyResult.Ok(faculty.ToDto());
    }

    public async Task<FacultyResult> UpdateAsync(int id, UpdateFacultyDto dto)
    {
        var faculty = await _dbContext.Faculties.FirstOrDefaultAsync(f => f.Id == id);
        if (faculty == null)
        {
            return FacultyResult.Fail(FacultyError.NotFound,
                $"Nie znaleziono wydziału o id {id}.");
        }
        
        var locationExists = await _dbContext.Locations.AnyAsync(l => l.Id == dto.LocationId);
        if (!locationExists)
        {
            return FacultyResult.Fail(FacultyError.LocationNotFound,
                $"Nie znaleziono lokalizacji o id {dto.LocationId}.");
        }
        
        var codeExists = await _dbContext.Faculties
            .AnyAsync(f => f.Code == dto.Code && f.Id != id);
        if (codeExists)
        {
            return FacultyResult.Fail(FacultyError.CodeAlreadyExists,
                $"Wydział o kodzie '{dto.Code}' już istnieje.");
        }

        faculty.ApplyUpdate(dto);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(faculty).Reference(f => f.Location).LoadAsync();

        return FacultyResult.Ok(faculty.ToDto());
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var faculty = await _dbContext.Faculties.FirstOrDefaultAsync(f => f.Id == id);
        if (faculty == null)
        {
            return false;
        }

        _dbContext.Faculties.Remove(faculty);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}