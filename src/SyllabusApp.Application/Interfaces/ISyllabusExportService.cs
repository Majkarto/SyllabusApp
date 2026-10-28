namespace SyllabusApp.Application.Interfaces;

/// <summary>Eksport sylabusa do plików. Zwraca null, gdy sylabus nie istnieje.</summary>
public interface ISyllabusExportService
{
    Task<byte[]?> ExportPdfAsync(int syllabusId);
    Task<byte[]?> ExportDocxAsync(int syllabusId);
}
