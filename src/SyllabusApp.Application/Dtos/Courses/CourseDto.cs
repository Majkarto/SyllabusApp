
namespace SyllabusApp.Application.Dtos.Courses;

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Semester { get; set; }
    
    public int FieldOfStudyId { get; set; }   // z FieldOfStudy
    public string FieldOfStudyName { get; set; } = string.Empty; 
    public string FieldOfStudyLevel { get; set; } = string.Empty;
    public int FieldOfStudyDurationSemesters { get; set; }  

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}