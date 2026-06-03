using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.StudentGroups;

public static class StudentGroupsMappingExtensions
{
    public static StudentGroupDto ToDto(this StudentGroup entity)
    {
        return new StudentGroupDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            CurrentSemester =  entity.CurrentSemester,
            StartYear = entity.StartYear,
            IsActive = entity.IsActive,
            FieldOfStudyId = entity.FieldOfStudyId,
            FieldOfStudyName = entity.FieldOfStudy?.Name ??  string.Empty,
        };
    }
    public static StudentGroup ToEntity(this CreateStudentGroupDto dto)
    {
        return new StudentGroup
        {
            Name = dto.Name,
            CurrentSemester =  dto.CurrentSemester,
            StartYear = dto.StartYear,
            IsActive = true,
            FieldOfStudyId = dto.FieldOfStudyId,

        };
    }
    public static void ApplyUpdate(this StudentGroup entity, UpdateStudentGroupDto dto)
    {
        entity.Name = dto.Name;
        entity.CurrentSemester = dto.CurrentSemester;
        entity.StartYear = dto.StartYear;
        entity.FieldOfStudyId = dto.FieldOfStudyId;
    }
}