using Microsoft.EntityFrameworkCore;
using SyllabusApp.Application.Dtos.Syllabuses;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Infrastructure.Persistence;

namespace SyllabusApp.Infrastructure.Services;

public class SyllabusService : ISyllabusService
{
    private readonly SyllabusDbContext _dbContext;

    public SyllabusService(SyllabusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<SyllabusDto>> GetAllAsync()
    {
        return await _dbContext.Syllabuses
            .AsNoTracking()
            .Include(s => s.Course)
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => s.ToDto())
            .ToListAsync();
    }

    public async Task<SyllabusDto?> GetByIdAsync(int id)
    {
        var syllabus = await _dbContext.Syllabuses
            .AsNoTracking()
            .Include(s => s.Course)
            .FirstOrDefaultAsync(s => s.Id == id);

        return syllabus?.ToDto();
    }

    public async Task<SyllabusResult> CreateAsync(CreateSyllabusDto dto, int lecturerId)
    {
        var courseExists = await _dbContext.Courses.AnyAsync(c => c.Id == dto.CourseId);
        if (!courseExists)
        {
            return SyllabusResult.Fail(SyllabusError.CourseNotFound,
                $"Nie znaleziono przedmiotu o id {dto.CourseId}.");
        }

        var syllabus = dto.ToEntity();
        syllabus.LecturerId = lecturerId;

        _dbContext.Syllabuses.Add(syllabus);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(syllabus).Reference(s => s.Course).LoadAsync();

        return SyllabusResult.Ok(syllabus.ToDto());
    }

    public async Task<SyllabusResult> UpdateAsync(int id, UpdateSyllabusDto dto)
    {
        var syllabus = await _dbContext.Syllabuses.FirstOrDefaultAsync(s => s.Id == id);
        if (syllabus == null)
        {
            return SyllabusResult.Fail(SyllabusError.NotFound,
                $"Nie znaleziono sylabusa o id {id}.");
        }

        var courseExists = await _dbContext.Courses.AnyAsync(c => c.Id == dto.CourseId);
        if (!courseExists)
        {
            return SyllabusResult.Fail(SyllabusError.CourseNotFound,
                $"Nie znaleziono przedmiotu o id {dto.CourseId}.");
        }

        syllabus.ApplyUpdate(dto);
        syllabus.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(syllabus).Reference(s => s.Course).LoadAsync();

        return SyllabusResult.Ok(syllabus.ToDto());
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var syllabus = await _dbContext.Syllabuses.FirstOrDefaultAsync(s => s.Id == id);
        if (syllabus == null)
        {
            return false;
        }

        _dbContext.Syllabuses.Remove(syllabus);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
