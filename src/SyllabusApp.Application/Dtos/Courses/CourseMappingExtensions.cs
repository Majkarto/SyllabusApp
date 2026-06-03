using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Application.Dtos.Courses;

public static class CourseMappingExtensions
{
    public static CourseDto ToDto(this Course entity)
    {
        return new CourseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            Semester = entity.Semester,
            FieldOfStudyId = entity.FieldOfStudyId,
            FieldOfStudyName =  entity.FieldOfStudy?.Name ?? string.Empty,
            FieldOfStudyLevel = entity.FieldOfStudy?.Level.ToString() ?? string.Empty,
            FieldOfStudyDurationSemesters = entity.FieldOfStudy?.DurationSemesters ?? 0,
            CreatedAt =  entity.CreatedAt,
            UpdatedAt =   entity.UpdatedAt,
        };
    }   

    public static Course ToEntity(this CreateCourseDto dto)
    {
        return new Course
        {
            Name = dto.Name,
            Code = dto.Code,
            Semester = dto.Semester,
            FieldOfStudyId = dto.FieldOfStudyId,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public static void ApplyUpdate(this Course entity, UpdateCourseDto dto)
    {
        entity.Name = dto.Name;
        entity.Code = dto.Code;
        entity.Semester = dto.Semester;
        entity.FieldOfStudyId = dto.FieldOfStudyId;
        entity.UpdatedAt = DateTime.UtcNow;
    }
}