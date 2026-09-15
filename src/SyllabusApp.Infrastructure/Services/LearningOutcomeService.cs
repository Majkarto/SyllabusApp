using Microsoft.EntityFrameworkCore;
using SyllabusApp.Application.Dtos.LearningOutcomes;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Infrastructure.Persistence;

namespace SyllabusApp.Infrastructure.Services;

public class LearningOutcomeService : ILearningOutcomeService
{
    private readonly SyllabusDbContext _dbContext;

    public LearningOutcomeService(SyllabusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<LearningOutcomeDto>> GetAllAsync()
    {
        return await _dbContext.LearningOutcomes
            .AsNoTracking()
            .Include(lo => lo.Syllabus)
            .OrderBy(lo => lo.Code)
            .Select(lo => lo.ToDto())
            .ToListAsync();
    }

    public async Task<LearningOutcomeDto?> GetByIdAsync(int id)
    {
        var learningOutcome = await _dbContext.LearningOutcomes
            .AsNoTracking()
            .Include(lo => lo.Syllabus)
            .FirstOrDefaultAsync(lo => lo.Id == id);

        return learningOutcome?.ToDto();
    }

    public async Task<LearningOutcomeResult> CreateAsync(CreateLearningOutcomeDto dto)
    {
        var syllabusExists = await _dbContext.Syllabuses.AnyAsync(s => s.Id == dto.SyllabusId);
        if (!syllabusExists)
        {
            return LearningOutcomeResult.Fail(LearningOutcomeError.SyllabusNotFound,
                $"Nie znaleziono sylabusa o id {dto.SyllabusId}.");
        }

        var learningOutcome = dto.ToEntity();
        _dbContext.LearningOutcomes.Add(learningOutcome);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(learningOutcome).Reference(lo => lo.Syllabus).LoadAsync();

        return LearningOutcomeResult.Ok(learningOutcome.ToDto());
    }

    public async Task<LearningOutcomeResult> UpdateAsync(int id, UpdateLearningOutcomeDto dto)
    {
        var learningOutcome = await _dbContext.LearningOutcomes.FirstOrDefaultAsync(lo => lo.Id == id);
        if (learningOutcome == null)
        {
            return LearningOutcomeResult.Fail(LearningOutcomeError.NotFound,
                $"Nie znaleziono efektu uczenia się o id {id}.");
        }

        var syllabusExists = await _dbContext.Syllabuses.AnyAsync(s => s.Id == dto.SyllabusId);
        if (!syllabusExists)
        {
            return LearningOutcomeResult.Fail(LearningOutcomeError.SyllabusNotFound,
                $"Nie znaleziono sylabusa o id {dto.SyllabusId}.");
        }

        learningOutcome.ApplyUpdate(dto);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(learningOutcome).Reference(lo => lo.Syllabus).LoadAsync();

        return LearningOutcomeResult.Ok(learningOutcome.ToDto());
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var learningOutcome = await _dbContext.LearningOutcomes.FirstOrDefaultAsync(lo => lo.Id == id);
        if (learningOutcome == null)
        {
            return false;
        }

        _dbContext.LearningOutcomes.Remove(learningOutcome);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
