using Microsoft.EntityFrameworkCore;
using SyllabusApp.Application.Dtos.Courses;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Infrastructure.Persistence;

namespace SyllabusApp.Infrastructure.Services;

public class CourseService : ICourseService
{
    private readonly SyllabusDbContext _dbContext;

    public CourseService(SyllabusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CourseDto>> GetAllAsync()
    {
        return await _dbContext.Courses
            .AsNoTracking()
            .Include(c => c.FieldOfStudy)
            .OrderBy(c => c.Name)
            .Select(c => c.ToDto())
            .ToListAsync();
    }

    public async Task<CourseDto?> GetByIdAsync(int id)
    {
        var course = await _dbContext.Courses
            .AsNoTracking()
            .Include(c => c.FieldOfStudy)
            .FirstOrDefaultAsync(c => c.Id == id);

        return course?.ToDto();
    }

    public async Task<CourseResult> CreateAsync(CreateCourseDto dto)
    {
        var fieldOfStudyExists = await _dbContext.FieldsOfStudy.AnyAsync(f => f.Id == dto.FieldOfStudyId);
        if (!fieldOfStudyExists)
        {
            return CourseResult.Fail(CourseError.FieldOfStudyNotFound,
                $"Nie znaleziono kierunku studiów o id {dto.FieldOfStudyId}.");
        }

        var codeExists = await _dbContext.Courses.AnyAsync(c => c.Code == dto.Code);
        if (codeExists)
        {
            return CourseResult.Fail(CourseError.CodeAlreadyExists,
                $"Przedmiot o kodzie '{dto.Code}' już istnieje.");
        }

        var course = dto.ToEntity();
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(course).Reference(c => c.FieldOfStudy).LoadAsync();

        return CourseResult.Ok(course.ToDto());
    }

    public async Task<CourseResult> UpdateAsync(int id, UpdateCourseDto dto)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
        if (course == null)
        {
            return CourseResult.Fail(CourseError.NotFound,
                $"Nie znaleziono przedmiotu o id {id}.");
        }

        var fieldOfStudyExists = await _dbContext.FieldsOfStudy.AnyAsync(f => f.Id == dto.FieldOfStudyId);
        if (!fieldOfStudyExists)
        {
            return CourseResult.Fail(CourseError.FieldOfStudyNotFound,
                $"Nie znaleziono kierunku studiów o id {dto.FieldOfStudyId}.");
        }

        var codeExists = await _dbContext.Courses
            .AnyAsync(c => c.Code == dto.Code && c.Id != id);
        if (codeExists)
        {
            return CourseResult.Fail(CourseError.CodeAlreadyExists,
                $"Przedmiot o kodzie '{dto.Code}' już istnieje.");
        }

        course.ApplyUpdate(dto);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(course).Reference(c => c.FieldOfStudy).LoadAsync();

        return CourseResult.Ok(course.ToDto());
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
        if (course == null)
        {
            return false;
        }

        _dbContext.Courses.Remove(course);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
