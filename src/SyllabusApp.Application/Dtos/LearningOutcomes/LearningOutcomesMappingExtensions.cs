using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Application.Dtos.LearningOutcomes;

public static class LearningOutcomesMappingExtensions
{
    public static LearningOutcomeDto ToDto(this LearningOutcome entity)
    {
        return new LearningOutcomeDto()
        {
            Id = entity.Id,
            Code = entity.Code,
            Description = entity.Description,
            Category = entity.Category,
            SyllabusId = entity.SyllabusId,
            SyllabusAcademicYear = entity.Syllabus.AcademicYear ?? string.Empty, 
        };
    }

    public static LearningOutcome ToEntity(this CreateLearningOutcomeDto dto)
    {
        return new LearningOutcome
        {
            Code = dto.Code,
            Description = dto.Description,
            Category = dto.Category,
            SyllabusId = dto.SyllabusId,
        };
    }
    
    public static void ApplyUpdate(this LearningOutcome entity, UpdateLearningOutcomeDto dto)
    {
        entity.Code = dto.Code;
        entity.Description = dto.Description;
        entity.Category = dto.Category;
        entity.SyllabusId = dto.SyllabusId;
        entity.UpdatedAt = DateTime.UtcNow;
    }
}