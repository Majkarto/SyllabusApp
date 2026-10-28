namespace SyllabusApp.Application.Interfaces;

public interface ISyllabusExportService
{
    Task<byte[]?> ExportPdfAsync(int syllabusId);
    Task<byte[]?> ExportDocxAsync(int syllabusId);
}
