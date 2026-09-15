using Microsoft.EntityFrameworkCore;
using SyllabusApp.Application.Dtos.StudentGroups;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Infrastructure.Persistence;

namespace SyllabusApp.Infrastructure.Services;

public class StudentGroupService : IStudentGroupService
{
    private readonly SyllabusDbContext _dbContext;

    public StudentGroupService(SyllabusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<StudentGroupDto>> GetAllAsync()
    {
        return await _dbContext.StudentGroups
            .AsNoTracking()
            .Include(g => g.FieldOfStudy)
            .OrderBy(g => g.Name)
            .Select(g => g.ToDto())
            .ToListAsync();
    }

    public async Task<StudentGroupDto?> GetByIdAsync(int id)
    {
        var studentGroup = await _dbContext.StudentGroups
            .AsNoTracking()
            .Include(g => g.FieldOfStudy)
            .FirstOrDefaultAsync(g => g.Id == id);

        return studentGroup?.ToDto();
    }

    public async Task<StudentGroupResult> CreateAsync(CreateStudentGroupDto dto)
    {
        var fieldOfStudyExists = await _dbContext.FieldsOfStudy.AnyAsync(f => f.Id == dto.FieldOfStudyId);
        if (!fieldOfStudyExists)
        {
            return StudentGroupResult.Fail(StudentGroupError.FieldOfStudyNotFound,
                $"Nie znaleziono kierunku studiów o id {dto.FieldOfStudyId}.");
        }

        var nameExists = await _dbContext.StudentGroups.AnyAsync(g => g.Name == dto.Name);
        if (nameExists)
        {
            return StudentGroupResult.Fail(StudentGroupError.NameAlreadyExists,
                $"Grupa studencka o nazwie '{dto.Name}' już istnieje.");
        }

        var studentGroup = dto.ToEntity();
        _dbContext.StudentGroups.Add(studentGroup);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(studentGroup).Reference(g => g.FieldOfStudy).LoadAsync();

        return StudentGroupResult.Ok(studentGroup.ToDto());
    }

    public async Task<StudentGroupResult> UpdateAsync(int id, UpdateStudentGroupDto dto)
    {
        var studentGroup = await _dbContext.StudentGroups.FirstOrDefaultAsync(g => g.Id == id);
        if (studentGroup == null)
        {
            return StudentGroupResult.Fail(StudentGroupError.NotFound,
                $"Nie znaleziono grupy studenckiej o id {id}.");
        }

        var fieldOfStudyExists = await _dbContext.FieldsOfStudy.AnyAsync(f => f.Id == dto.FieldOfStudyId);
        if (!fieldOfStudyExists)
        {
            return StudentGroupResult.Fail(StudentGroupError.FieldOfStudyNotFound,
                $"Nie znaleziono kierunku studiów o id {dto.FieldOfStudyId}.");
        }

        var nameExists = await _dbContext.StudentGroups
            .AnyAsync(g => g.Name == dto.Name && g.Id != id);
        if (nameExists)
        {
            return StudentGroupResult.Fail(StudentGroupError.NameAlreadyExists,
                $"Grupa studencka o nazwie '{dto.Name}' już istnieje.");
        }

        studentGroup.ApplyUpdate(dto);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(studentGroup).Reference(g => g.FieldOfStudy).LoadAsync();

        return StudentGroupResult.Ok(studentGroup.ToDto());
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var studentGroup = await _dbContext.StudentGroups.FirstOrDefaultAsync(g => g.Id == id);
        if (studentGroup == null)
        {
            return false;
        }

        _dbContext.StudentGroups.Remove(studentGroup);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
