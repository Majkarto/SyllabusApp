using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.SyllabusApprovals;

public class SyllabusApprovalDto
{
    public int Id { get; set; }
    public ApprovalAction Action { get; set; }
    public string? Comment { get; set; }
    public DateTime ActionDate { get; set; } = DateTime.UtcNow;
    public int SyllabusId { get; set; }
    public string SyllabusAcademicYear { get; set; } = string.Empty;
    public SyllabusStatus SyllabusStatus { get; set; }
}