namespace SyllabusApp.Application.Dtos.Curriculum;

/// <summary>Siatka przedmiotów kierunku: semestry z przedmiotami i godzinami.</summary>
public class CurriculumGridDto
{
    public int FieldOfStudyId { get; set; }
    public string FieldOfStudyName { get; set; } = string.Empty;
    public string? AcademicYear { get; set; }
    public List<CurriculumSemesterDto> Semesters { get; set; } = [];
    public int TotalHours => Semesters.Sum(s => s.TotalHours);
    public int TotalEcts => Semesters.Sum(s => s.TotalEcts);
}

public class CurriculumSemesterDto
{
    public int Semester { get; set; }
    public List<CurriculumCourseDto> Courses { get; set; } = [];
    public int TotalHours => Courses.Sum(c => c.TotalHours);
    public int TotalEcts => Courses.Sum(c => c.EctsPoints);
}

public class CurriculumCourseDto
{
    public int CourseId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    /// <summary>Null, gdy przedmiot nie ma jeszcze sylabusa.</summary>
    public int? SyllabusId { get; set; }
    public int LectureHours { get; set; }
    public int ExerciseHours { get; set; }
    public int LaboratoryHours { get; set; }
    public int EctsPoints { get; set; }
    public int TotalHours => LectureHours + ExerciseHours + LaboratoryHours;
}
