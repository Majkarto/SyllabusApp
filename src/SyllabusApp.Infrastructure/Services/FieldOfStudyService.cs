using Microsoft.EntityFrameworkCore;
using SyllabusApp.Application.Dtos.FieldOfStudies;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Infrastructure.Persistence;

namespace SyllabusApp.Infrastructure.Services;

public class FieldOfStudyService : IFieldOfStudyService
{
    private readonly SyllabusDbContext _dbContext;

    public FieldOfStudyService(SyllabusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<FieldOfStudyDto>> GetAllAsync()
    {
        return await _dbContext.FieldsOfStudy
            .AsNoTracking()
            .Include(f => f.Faculty)
            .OrderBy(f => f.Name)
            .Select(f => f.ToDto())
            .ToListAsync();
    }

    public async Task<FieldOfStudyDto?> GetByIdAsync(int id)
    {
        var fieldOfStudy = await _dbContext.FieldsOfStudy
            .AsNoTracking()
            .Include(f => f.Faculty)
            .FirstOrDefaultAsync(f => f.Id == id);

        return fieldOfStudy?.ToDto();
    }

    public async Task<FieldOfStudyResult> CreateAsync(CreateFieldOfStudyDto dto)
    {
        var facultyExists = await _dbContext.Faculties.AnyAsync(f => f.Id == dto.FacultyId);
        if (!facultyExists)
        {
            return FieldOfStudyResult.Fail(FieldOfStudyError.FacultyNotFound,
                $"Nie znaleziono wydziału o id {dto.FacultyId}.");
        }

        var codeExists = await _dbContext.FieldsOfStudy.AnyAsync(f => f.Code == dto.Code);
        if (codeExists)
        {
            return FieldOfStudyResult.Fail(FieldOfStudyError.CodeAlreadyExists,
                $"Kierunek studiów o kodzie '{dto.Code}' już istnieje.");
        }

        var fieldOfStudy = dto.ToEntity();
        _dbContext.FieldsOfStudy.Add(fieldOfStudy);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(fieldOfStudy).Reference(f => f.Faculty).LoadAsync();

        return FieldOfStudyResult.Ok(fieldOfStudy.ToDto());
    }

    public async Task<FieldOfStudyResult> UpdateAsync(int id, UpdateFieldOfStudyDto dto)
    {
        var fieldOfStudy = await _dbContext.FieldsOfStudy.FirstOrDefaultAsync(f => f.Id == id);
        if (fieldOfStudy == null)
        {
            return FieldOfStudyResult.Fail(FieldOfStudyError.NotFound,
                $"Nie znaleziono kierunku studiów o id {id}.");
        }

        var facultyExists = await _dbContext.Faculties.AnyAsync(f => f.Id == dto.FacultyId);
        if (!facultyExists)
        {
            return FieldOfStudyResult.Fail(FieldOfStudyError.FacultyNotFound,
                $"Nie znaleziono wydziału o id {dto.FacultyId}.");
        }

        var codeExists = await _dbContext.FieldsOfStudy
            .AnyAsync(f => f.Code == dto.Code && f.Id != id);
        if (codeExists)
        {
            return FieldOfStudyResult.Fail(FieldOfStudyError.CodeAlreadyExists,
                $"Kierunek studiów o kodzie '{dto.Code}' już istnieje.");
        }

        fieldOfStudy.ApplyUpdate(dto);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(fieldOfStudy).Reference(f => f.Faculty).LoadAsync();

        return FieldOfStudyResult.Ok(fieldOfStudy.ToDto());
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var fieldOfStudy = await _dbContext.FieldsOfStudy.FirstOrDefaultAsync(f => f.Id == id);
        if (fieldOfStudy == null)
        {
            return false;
        }

        _dbContext.FieldsOfStudy.Remove(fieldOfStudy);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
