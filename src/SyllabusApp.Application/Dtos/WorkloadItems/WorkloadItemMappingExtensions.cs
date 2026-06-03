using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.WorkloadItems;

public static class WorkloadItemMappingExtensions
{
    
    public static WorkloadItemDto ToDto(this WorkloadItem entity)
    {
        return new WorkloadItemDto()
        {
            Id = entity.Id,
            Description =  entity.Description,
            Hours =  entity.Hours,
            Type = entity.Type,
            SyllabusId = entity.SyllabusId,
            SyllabusAcademicYear = entity.Syllabus?.AcademicYear ??  string.Empty,
        };
    }
    public static WorkloadItem ToEntity(this CreateWorkloadItemDto dto)
    {
        return new WorkloadItem
        {
            Description =  dto.Description,
            Hours =  dto.Hours,
            Type = dto.Type,
            SyllabusId = dto.SyllabusId,
        };
    }
    public static void ApplyUpdate(this WorkloadItem entity, UpdateWorkloadItemDto dto)
    {
        entity.Description = dto.Description;
        entity.Hours = dto.Hours;
        entity.Type = dto.Type;
        entity.SyllabusId = dto.SyllabusId;
    }
}