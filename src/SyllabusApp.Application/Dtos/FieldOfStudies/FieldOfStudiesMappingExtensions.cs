using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Application.Dtos.FieldOfStudies;

public static class FieldOfStudiesMappingExtensions
{
    public static FieldOfStudyDto ToDto(this FieldOfStudy entity)
    {
        return new FieldOfStudyDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            Level = entity.Level,
            DurationSemesters = entity.DurationSemesters,
            FacultyId = entity.FacultyId,
            FacultyName = entity.Faculty?.Name ?? string.Empty,
            FacultyCode = entity.Faculty?.Code ?? string.Empty,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }

    public static FieldOfStudy ToEntity(this CreateFieldOfStudyDto dto)
    {
        return new FieldOfStudy
        {
            Name = dto.Name,
            Code = dto.Code,
            Level = dto.Level,
            DurationSemesters = dto.DurationSemesters,
            FacultyId = dto.FacultyId,
            CreatedAt = DateTime.UtcNow,
        };
    }
    
    public static void ApplyUpdate(this FieldOfStudy entity, UpdateFieldOfStudyDto dto)
    {
        entity.Name = dto.Name;
        entity.Code = dto.Code;
        entity.Level = dto.Level;
        entity.DurationSemesters = dto.DurationSemesters;
        entity.FacultyId = dto.FacultyId;
        entity.UpdatedAt = DateTime.UtcNow;
    }
}