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
            .Include(f => f.Name)
            .OrderBy(f => f.Faculty.Name)
            .Select(f => f.ToDto())
            .ToListAsync();
    }

    public async Task<FieldOfStudyDto?> GetByIdAsync(int id)
    {
        var faculty = await _dbContext.FieldsOfStudy
            .AsNoTracking()
            .Include(f => f.Faculty.Name)
            .FirstOrDefaultAsync(f => f.Id == id);

        return faculty?.ToDto();
    }

    public async Task<FieldOfStudyResult> CreateAsync(CreateFieldOfStudyDto dto)
    {
        var facultyExists = await _dbContext.Faculties.AnyAsync(l => l.Id == dto.FacultyId);
        if (!facultyExists)
        {
            return FieldOfStudyError.Fail(FieldOfStudyError.facyltyNotFound,
                $"Nie znaleziono wydziału o id {dto.FacultyId}.");
        }
        
        var codeExists = await _dbContext.FieldsOfStudy.AnyAsync(f => f.Code == dto.Code);
        if (codeExists)
        {
            return FieldOfStudyResult.Fail(FieldOfStudyError.CodeAlreadyExists,
                $"kierunek nauki o kodzie '{dto.Code}' już istnieje.");
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
                $"Nie znaleziono kierunku nauki o id {id}.");
        }
        
        var facultyExists = await _dbContext.Faculties.AnyAsync(l => l.Id == dto.FacultyId);
        if (!facultyExists)
        {
            return FieldOfStudyResult.Fail(FieldOfStudyError.FaculityNotFound,
                $"Nie znaleziono placówki o id {dto.FacultyId}.");
        }
        
        var codeExists = await _dbContext.FieldsOfStudy
            .AnyAsync(f => f.Code == dto.Code && f.Id != id);
        if (codeExists)
        {
            return FieldOfStudyResult.Fail(FieldOfStudyError.CodeAlreadyExists,
                $"kierunek nauki o kodzie '{dto.Code}' już istnieje.");
        }

        fieldOfStudy.ApplyUpdate(dto);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(fieldOfStudy).Reference(f => f.Faculty).LoadAsync();

        return FieldOfStudyResult.Ok(fieldOfStudy.ToDto());
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var fieldOfStudyResult = await _dbContext.FieldsOfStudy.FirstOrDefaultAsync(f => f.Id == id);
        if (fieldOfStudyResult == null)
        {
            return false;
        }

        _dbContext.FieldsOfStudy.Remove(fieldOfStudyResult);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}