using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.Syllabuses;

public static class SyllabusMappingExtensions
{
    public static SyllabusDto ToDto(this Syllabus entity)
    {
        return new SyllabusDto()
        {
            Id = entity.Id,
            AcademicYear =  entity.AcademicYear,
            EctsPoints =  entity.EctsPoints,
            LectureHours =  entity.LectureHours,
            ExerciseHours =  entity.ExerciseHours,
            LaboratoryHours =  entity.LaboratoryHours,
            CourseObjectives =  entity.CourseObjectives,
            AssessmentMethod =   entity.AssessmentMethod,
            Prerequisites =  entity.Prerequisites,
            Status =  entity.Status,
            CourseId =  entity.CourseId,
            CourseName = entity.Course?.Name ?? string.Empty,
        };
    }
    public static Syllabus ToEntity(this CreateSyllabusDto dto)
    {
        return new Syllabus
        {
            AcademicYear =  dto.AcademicYear,
            EctsPoints =  dto.EctsPoints,
            LectureHours =  dto.LectureHours,
            ExerciseHours =  dto.ExerciseHours,
            LaboratoryHours =  dto.LaboratoryHours,
            CourseObjectives =  dto.CourseObjectives,
            AssessmentMethod =   dto.AssessmentMethod,
            Prerequisites =  dto.Prerequisites,
            Status = SyllabusStatus.Draft,
            CourseId =  dto.CourseId,
        };
    }
    public static void ApplyUpdate(this Syllabus entity, UpdateSyllabusDto dto)
    {
        entity.AcademicYear = dto.AcademicYear;
        entity.EctsPoints = dto.EctsPoints;
        entity.LectureHours = dto.LectureHours;
        entity.ExerciseHours = dto.ExerciseHours;
        entity.LaboratoryHours = dto.LaboratoryHours;
        entity.CourseObjectives = dto.CourseObjectives;
        entity.AssessmentMethod = dto.AssessmentMethod;
        entity.Prerequisites = dto.Prerequisites;
        entity.CourseId = dto.CourseId;
    }
}