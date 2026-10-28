using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.Syllabuses;

public static class SyllabusMappingExtensions
{
    public static SyllabusDto ToDto(this Syllabus entity)
    {
        var fieldOfStudy = entity.Course?.FieldOfStudy;
        var location = fieldOfStudy?.Faculty?.Location;

        return new SyllabusDto
        {
            Id = entity.Id,
            AcademicYear = entity.AcademicYear,
            EctsPoints = entity.EctsPoints,
            LectureHours = entity.LectureHours,
            ExerciseHours = entity.ExerciseHours,
            LaboratoryHours = entity.LaboratoryHours,
            Description = entity.Description,
            CourseObjectives = entity.CourseObjectives,
            AssessmentMethod = entity.AssessmentMethod,
            Prerequisites = entity.Prerequisites,
            Status = entity.Status,
            CourseId = entity.CourseId,
            CourseName = entity.Course?.Name ?? string.Empty,
            CourseCode = entity.Course?.Code ?? string.Empty,
            Semester = entity.Course?.Semester ?? 0,
            FieldOfStudyId = fieldOfStudy?.Id,
            FieldOfStudyName = fieldOfStudy?.Name ?? string.Empty,
            LocationId = location?.Id,
            LocationName = location?.Name ?? string.Empty,
            CurrentVersion = entity.Versions.Count == 0 ? 0 : entity.Versions.Max(v => v.VersionNumber),
        };
    }

    public static Syllabus ToEntity(this CreateSyllabusDto dto)
    {
        var entity = new Syllabus { Status = SyllabusStatus.Draft };
        entity.ApplyFields(dto);
        return entity;
    }

    public static void ApplyUpdate(this Syllabus entity, UpdateSyllabusDto dto) => entity.ApplyFields(dto);

    private static void ApplyFields(this Syllabus entity, SyllabusEditDto dto)
    {
        entity.AcademicYear = dto.AcademicYear;
        entity.EctsPoints = dto.EctsPoints;
        entity.LectureHours = dto.LectureHours;
        entity.ExerciseHours = dto.ExerciseHours;
        entity.LaboratoryHours = dto.LaboratoryHours;
        entity.Description = dto.Description;
        entity.CourseObjectives = dto.CourseObjectives;
        entity.AssessmentMethod = dto.AssessmentMethod;
        entity.Prerequisites = dto.Prerequisites;
        entity.CourseId = dto.CourseId;
    }

    /// <summary>Tworzy wpis historii z aktualnego stanu sylabusa.</summary>
    public static SyllabusVersion ToVersion(this Syllabus entity, int versionNumber, string changeDescription,
        int? createdById, string snapshotJson)
    {
        return new SyllabusVersion
        {
            SyllabusId = entity.Id,
            VersionNumber = versionNumber,
            ChangeDescription = changeDescription,
            AcademicYear = entity.AcademicYear,
            EctsPoints = entity.EctsPoints,
            LectureHours = entity.LectureHours,
            ExerciseHours = entity.ExerciseHours,
            LaboratoryHours = entity.LaboratoryHours,
            CourseObjectives = entity.CourseObjectives,
            AssessmentMethod = entity.AssessmentMethod,
            Prerequisites = entity.Prerequisites,
            SnapshotJson = snapshotJson,
            CreatedById = createdById,
            CreatedAt = DateTime.UtcNow,
        };
    }
}
