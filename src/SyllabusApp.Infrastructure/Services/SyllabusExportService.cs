using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Domain.Entities;
using SyllabusApp.Infrastructure.Persistence;

namespace SyllabusApp.Infrastructure.Services;

/// <summary>
/// Eksport sylabusa do PDF (QuestPDF) i DOCX (Open XML SDK).
/// Oba formaty mają tę samą treść: dane podstawowe, opisy, efekty, literaturę i nakład pracy.
/// </summary>
public class SyllabusExportService : ISyllabusExportService
{
    private readonly SyllabusDbContext _dbContext;

    static SyllabusExportService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public SyllabusExportService(SyllabusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private Task<Syllabus?> LoadAsync(int id) => _dbContext.Syllabuses
        .AsNoTracking()
        .Include(s => s.Course).ThenInclude(c => c.FieldOfStudy).ThenInclude(f => f.Faculty).ThenInclude(f => f.Location)
        .Include(s => s.Lecturer)
        .Include(s => s.LearningOutcomes)
        .Include(s => s.Literature)
        .Include(s => s.WorkloadItems)
        .FirstOrDefaultAsync(s => s.Id == id);

    // Wspólna treść dla obu formatów

    private static List<(string Label, string Value)> BasicInfo(Syllabus s) =>
    [
        ("Przedmiot", $"{s.Course.Name} ({s.Course.Code})"),
        ("Kierunek", s.Course.FieldOfStudy?.Name ?? "-"),
        ("Wydział", s.Course.FieldOfStudy?.Faculty?.Name ?? "-"),
        ("Ośrodek", s.Course.FieldOfStudy?.Faculty?.Location?.Name ?? "-"),
        ("Rok akademicki", s.AcademicYear),
        ("Semestr", s.Course.Semester.ToString()),
        ("Punkty ECTS", s.EctsPoints.ToString()),
        ("Godziny", $"wykłady {s.LectureHours} h, ćwiczenia {s.ExerciseHours} h, laboratoria {s.LaboratoryHours} h"),
        ("Prowadzący", s.Lecturer?.FullName ?? "-"),
        ("Status", StatusLabel(s.Status)),
    ];

    private static List<(string Title, string Text)> TextSections(Syllabus s) =>
    [
        ("Opis przedmiotu", s.Description),
        ("Cele kształcenia", s.CourseObjectives),
        ("Wymagania wstępne", s.Prerequisites),
        ("Metody oceniania", s.AssessmentMethod),
    ];

    private static List<string[]> OutcomeRows(Syllabus s) => s.LearningOutcomes
        .OrderBy(o => o.Code)
        .Select(o => new[] { o.Code, CategoryLabel(o.Category), o.Description })
        .ToList();

    private static List<string[]> LiteratureRows(Syllabus s) => s.Literature
        .OrderBy(l => l.Type).ThenBy(l => l.Author)
        .Select(l => new[]
        {
            l.Type == LiteratureType.Required ? "Podstawowa" : "Uzupełniająca",
            l.Author,
            l.Title,
            string.Join(", ", new[] { l.Publisher, l.PublicationYear?.ToString() }.Where(x => !string.IsNullOrEmpty(x)))
        })
        .ToList();

    private static List<string[]> WorkloadRows(Syllabus s)
    {
        var rows = s.WorkloadItems
            .OrderBy(w => w.Type)
            .Select(w => new[] { WorkloadLabel(w.Type), w.Description, w.Hours.ToString() })
            .ToList();
        rows.Add(["", "Razem", s.WorkloadItems.Sum(w => w.Hours).ToString()]);
        return rows;
    }

    private static readonly string[] OutcomeHeader = ["Kod", "Kategoria", "Opis"];
    private static readonly string[] LiteratureHeader = ["Rodzaj", "Autor", "Tytuł", "Wydawnictwo, rok"];
    private static readonly string[] WorkloadHeader = ["Rodzaj", "Opis", "Godziny"];

    private static string StatusLabel(SyllabusStatus status) => status switch
    {
        SyllabusStatus.Draft => "Szkic",
        SyllabusStatus.UnderReview => "W recenzji",
        SyllabusStatus.Approved => "Zatwierdzony",
        SyllabusStatus.Published => "Opublikowany",
        SyllabusStatus.Archived => "Zarchiwizowany",
        _ => status.ToString()
    };

    private static string CategoryLabel(LearningOutcomeCategory c) => c switch
    {
        LearningOutcomeCategory.Knowledge => "Wiedza",
        LearningOutcomeCategory.Skills => "Umiejętności",
        LearningOutcomeCategory.Social => "Kompetencje społeczne",
        _ => c.ToString()
    };

    private static string WorkloadLabel(WorkloadType t) => t switch
    {
        WorkloadType.Contact => "Kontaktowe",
        WorkloadType.Independent => "Praca własna",
        WorkloadType.Assessment => "Zaliczenie",
        _ => t.ToString()
    };

    // PDF

    public async Task<byte[]?> ExportPdfAsync(int syllabusId)
    {
        var s = await LoadAsync(syllabusId);
        if (s == null) return null;

        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Text($"Sylabus: {s.Course.Name}").FontSize(18).Bold();

                page.Content().PaddingVertical(10).Column(col =>
                {
                    col.Spacing(8);

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c => { c.ConstantColumn(120); c.RelativeColumn(); });
                        foreach (var (label, value) in BasicInfo(s))
                        {
                            table.Cell().BorderBottom(0.5f).Padding(3).Text(label).Bold();
                            table.Cell().BorderBottom(0.5f).Padding(3).Text(value);
                        }
                    });

                    foreach (var (title, text) in TextSections(s))
                    {
                        col.Item().Text(title).FontSize(13).Bold();
                        col.Item().Text(string.IsNullOrWhiteSpace(text) ? "-" : text);
                    }

                    AddPdfTable(col, "Efekty uczenia się", OutcomeHeader, OutcomeRows(s));
                    AddPdfTable(col, "Literatura", LiteratureHeader, LiteratureRows(s));
                    AddPdfTable(col, "Nakład pracy studenta", WorkloadHeader, WorkloadRows(s));
                });

                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Strona ");
                    t.CurrentPageNumber();
                    t.Span(" z ");
                    t.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    private static void AddPdfTable(ColumnDescriptor col, string title, string[] header, List<string[]> rows)
    {
        col.Item().Text(title).FontSize(13).Bold();
        if (rows.Count == 0)
        {
            col.Item().Text("-");
            return;
        }

        col.Item().Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                foreach (var _ in header) c.RelativeColumn();
            });
            foreach (var h in header)
                table.Cell().Background(Colors.Grey.Lighten2).Border(0.5f).Padding(3).Text(h).Bold();
            foreach (var row in rows)
                foreach (var cell in row)
                    table.Cell().Border(0.5f).Padding(3).Text(cell);
        });
    }

    // DOCX

    public async Task<byte[]?> ExportDocxAsync(int syllabusId)
    {
        var s = await LoadAsync(syllabusId);
        if (s == null) return null;

        using var stream = new MemoryStream();
        using (var doc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
        {
            var mainPart = doc.AddMainDocumentPart();
            var body = new Body();
            mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(body);

            body.Append(DocxParagraph($"Sylabus: {s.Course.Name}", bold: true, size: 36));

            body.Append(DocxTable(null, BasicInfo(s).Select(x => new[] { x.Label, x.Value }).ToList()));

            foreach (var (title, text) in TextSections(s))
            {
                body.Append(DocxParagraph(title, bold: true, size: 26));
                body.Append(DocxParagraph(string.IsNullOrWhiteSpace(text) ? "-" : text));
            }

            AddDocxTable(body, "Efekty uczenia się", OutcomeHeader, OutcomeRows(s));
            AddDocxTable(body, "Literatura", LiteratureHeader, LiteratureRows(s));
            AddDocxTable(body, "Nakład pracy studenta", WorkloadHeader, WorkloadRows(s));

            mainPart.Document.Save();
        }

        return stream.ToArray();
    }

    private static void AddDocxTable(Body body, string title, string[] header, List<string[]> rows)
    {
        body.Append(DocxParagraph(title, bold: true, size: 26));
        if (rows.Count == 0)
        {
            body.Append(DocxParagraph("-"));
            return;
        }
        body.Append(DocxTable(header, rows));
    }

    private static Paragraph DocxParagraph(string text, bool bold = false, int size = 20)
    {
        var props = new RunProperties(new FontSize { Val = size.ToString() });
        if (bold) props.Append(new Bold());
        return new Paragraph(new Run(props, new Text(text) { Space = SpaceProcessingModeValues.Preserve }));
    }

    private static Table DocxTable(string[]? header, List<string[]> rows)
    {
        var table = new Table();
        var border = new EnumValue<BorderValues>(BorderValues.Single);
        table.Append(new TableProperties(
            new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct },
            new TableBorders(
                new TopBorder { Val = border, Size = 4 },
                new BottomBorder { Val = border, Size = 4 },
                new LeftBorder { Val = border, Size = 4 },
                new RightBorder { Val = border, Size = 4 },
                new InsideHorizontalBorder { Val = border, Size = 4 },
                new InsideVerticalBorder { Val = border, Size = 4 })));

        if (header != null)
            table.Append(new TableRow(header.Select(h => new TableCell(DocxParagraph(h, bold: true)))));
        foreach (var row in rows)
            table.Append(new TableRow(row.Select(c => new TableCell(DocxParagraph(c)))));

        return table;
    }
}
