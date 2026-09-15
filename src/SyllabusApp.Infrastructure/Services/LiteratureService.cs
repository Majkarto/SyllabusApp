using Microsoft.EntityFrameworkCore;
using SyllabusApp.Application.Dtos.Literatures;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Infrastructure.Persistence;

namespace SyllabusApp.Infrastructure.Services;

public class LiteratureService : ILiteratureService
{
    private readonly SyllabusDbContext _dbContext;

    public LiteratureService(SyllabusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<LiteratureDto>> GetAllAsync()
    {
        return await _dbContext.Literature
            .AsNoTracking()
            .OrderBy(l => l.Title)
            .Select(l => l.ToDto())
            .ToListAsync();
    }

    public async Task<LiteratureDto?> GetByIdAsync(int id)
    {
        var literature = await _dbContext.Literature
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);

        return literature?.ToDto();
    }

    public async Task<LiteratureResult> CreateAsync(CreateLiteratureDto dto)
    {
        var syllabusExists = await _dbContext.Syllabuses.AnyAsync(s => s.Id == dto.SyllabusId);
        if (!syllabusExists)
        {
            return LiteratureResult.Fail(LiteratureError.SyllabusNotFound,
                $"Nie znaleziono sylabusa o id {dto.SyllabusId}.");
        }

        var literature = dto.ToEntity();
        _dbContext.Literature.Add(literature);
        await _dbContext.SaveChangesAsync();

        return LiteratureResult.Ok(literature.ToDto());
    }

    public async Task<LiteratureResult> UpdateAsync(int id, UpdateLiteratureDto dto)
    {
        var literature = await _dbContext.Literature.FirstOrDefaultAsync(l => l.Id == id);
        if (literature == null)
        {
            return LiteratureResult.Fail(LiteratureError.NotFound,
                $"Nie znaleziono pozycji literatury o id {id}.");
        }

        var syllabusExists = await _dbContext.Syllabuses.AnyAsync(s => s.Id == dto.SyllabusId);
        if (!syllabusExists)
        {
            return LiteratureResult.Fail(LiteratureError.SyllabusNotFound,
                $"Nie znaleziono sylabusa o id {dto.SyllabusId}.");
        }

        literature.ApplyUpdate(dto);
        await _dbContext.SaveChangesAsync();

        return LiteratureResult.Ok(literature.ToDto());
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var literature = await _dbContext.Literature.FirstOrDefaultAsync(l => l.Id == id);
        if (literature == null)
        {
            return false;
        }

        _dbContext.Literature.Remove(literature);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
