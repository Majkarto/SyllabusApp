using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SyllabusApp.Application.Dtos.Curriculum;
using SyllabusApp.Application.Dtos.Syllabuses;
using SyllabusApp.Application.Dtos.SyllabusVersions;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Domain.Entities;
using SyllabusApp.Infrastructure.Persistence;

namespace SyllabusApp.Infrastructure.Services;

public class SyllabusService : ISyllabusService
{
    private readonly SyllabusDbContext _dbContext;

    public SyllabusService(SyllabusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private IQueryable<Syllabus> SyllabusesWithDetails() => _dbContext.Syllabuses
        .Include(s => s.Course).ThenInclude(c => c.FieldOfStudy).ThenInclude(f => f.Faculty).ThenInclude(f => f.Location)
        .Include(s => s.Versions);

    public async Task<IEnumerable<SyllabusDto>> GetAllAsync(SyllabusFilterDto? filter = null)
    {
        var query = SyllabusesWithDetails().AsNoTracking();

        if (filter != null)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim().ToLower();
                query = query.Where(s =>
                    s.Course.Name.ToLower().Contains(term) ||
                    s.Course.Code.ToLower().Contains(term) ||
                    s.Description.ToLower().Contains(term) ||
                    s.CourseObjectives.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(filter.AcademicYear))
                query = query.Where(s => s.AcademicYear == filter.AcademicYear);
            if (filter.Status.HasValue)
                query = query.Where(s => s.Status == filter.Status.Value);
            if (filter.FieldOfStudyId.HasValue)
                query = query.Where(s => s.Course.FieldOfStudyId == filter.FieldOfStudyId.Value);
            if (filter.LocationId.HasValue)
                query = query.Where(s => s.Course.FieldOfStudy.Faculty.LocationId == filter.LocationId.Value);
            if (filter.Semester.HasValue)
                query = query.Where(s => s.Course.Semester == filter.Semester.Value);
        }

        var syllabuses = await query.OrderByDescending(s => s.CreatedAt).ToListAsync();
        return syllabuses.Select(s => s.ToDto()).ToList();
    }

    public async Task<SyllabusDto?> GetByIdAsync(int id)
    {
        var syllabus = await SyllabusesWithDetails().AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
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

        await AddVersionAsync(syllabus, 1, "Utworzenie sylabusa", lecturerId);

        return SyllabusResult.Ok((await GetByIdAsync(syllabus.Id))!);
    }

    public async Task<SyllabusResult> UpdateAsync(int id, UpdateSyllabusDto dto, int? userId = null)
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

        var lastVersion = await _dbContext.SyllabusVersions
            .Where(v => v.SyllabusId == id)
            .MaxAsync(v => (int?)v.VersionNumber) ?? 0;
        var description = string.IsNullOrWhiteSpace(dto.ChangeDescription) ? "Aktualizacja sylabusa" : dto.ChangeDescription.Trim();
        await AddVersionAsync(syllabus, lastVersion + 1, description, userId);

        return SyllabusResult.Ok((await GetByIdAsync(id))!);
    }

    private async Task AddVersionAsync(Syllabus syllabus, int versionNumber, string description, int? userId)
    {
        await _dbContext.Entry(syllabus).Reference(s => s.Course).LoadAsync();
        var snapshotJson = JsonSerializer.Serialize(syllabus.ToDto());

        _dbContext.SyllabusVersions.Add(syllabus.ToVersion(versionNumber, description, userId, snapshotJson));
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<SyllabusVersionDto>?> GetVersionsAsync(int syllabusId)
    {
        if (!await _dbContext.Syllabuses.AnyAsync(s => s.Id == syllabusId))
        {
            return null;
        }

        var versions = await _dbContext.SyllabusVersions
            .AsNoTracking()
            .Include(v => v.CreatedBy)
            .Where(v => v.SyllabusId == syllabusId)
            .OrderByDescending(v => v.VersionNumber)
            .ToListAsync();

        return versions.Select(v => new SyllabusVersionDto
        {
            Id = v.Id,
            VersionNumber = v.VersionNumber,
            ChangeDescription = v.ChangeDescription,
            AcademicYear = v.AcademicYear,
            EctsPoints = v.EctsPoints,
            LectureHours = v.LectureHours,
            ExerciseHours = v.ExerciseHours,
            LaboratoryHours = v.LaboratoryHours,
            CourseObjectives = v.CourseObjectives,
            AssessmentMethod = v.AssessmentMethod,
            Prerequisites = v.Prerequisites,
            SnapshotJson = v.SnapshotJson,
            SyllabusId = v.SyllabusId,
            CreatedAt = v.CreatedAt,
            CreatedByName = v.CreatedBy == null ? null : $"{v.CreatedBy.FirstName} {v.CreatedBy.LastName}",
        }).ToList();
    }

    public async Task<CurriculumGridDto?> GetCurriculumGridAsync(int fieldOfStudyId, string? academicYear)
    {
        var fieldOfStudy = await _dbContext.FieldsOfStudy.AsNoTracking().FirstOrDefaultAsync(f => f.Id == fieldOfStudyId);
        if (fieldOfStudy == null)
        {
            return null;
        }

        var courses = await _dbContext.Courses
            .AsNoTracking()
            .Include(c => c.Syllabuses)
            .Where(c => c.FieldOfStudyId == fieldOfStudyId)
            .ToListAsync();

        var semesters = courses
            .GroupBy(c => c.Semester)
            .OrderBy(g => g.Key)
            .Select(g => new CurriculumSemesterDto
            {
                Semester = g.Key,
                Courses = g.OrderBy(c => c.Code).Select(c =>
                {
                    var syllabus = string.IsNullOrWhiteSpace(academicYear)
                        ? c.Syllabuses.OrderByDescending(s => s.AcademicYear).ThenByDescending(s => s.CreatedAt).FirstOrDefault()
                        : c.Syllabuses.FirstOrDefault(s => s.AcademicYear == academicYear);

                    return new CurriculumCourseDto
                    {
                        CourseId = c.Id,
                        CourseCode = c.Code,
                        CourseName = c.Name,
                        SyllabusId = syllabus?.Id,
                        LectureHours = syllabus?.LectureHours ?? 0,
                        ExerciseHours = syllabus?.ExerciseHours ?? 0,
                        LaboratoryHours = syllabus?.LaboratoryHours ?? 0,
                        EctsPoints = syllabus?.EctsPoints ?? 0,
                    };
                }).ToList()
            }).ToList();

        return new CurriculumGridDto
        {
            FieldOfStudyId = fieldOfStudy.Id,
            FieldOfStudyName = fieldOfStudy.Name,
            AcademicYear = academicYear,
            Semesters = semesters,
        };
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
