using Microsoft.EntityFrameworkCore;
using SyllabusApp.Application.Dtos.Syllabuses;
using SyllabusApp.Domain.Entities;
using SyllabusApp.Infrastructure.Persistence;
using SyllabusApp.Infrastructure.Services;

namespace SyllabusApp.Tests;

public class SyllabusServiceTests
{
    private const int LecturerId = 1;

    private static SyllabusDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<SyllabusDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var db = new SyllabusDbContext(options);

        var lodz = new Location { Id = 1, Name = "Łódź", Address = "Kilińskiego 109", City = "Łódź", PostalCode = "90-011" };
        var warsaw = new Location { Id = 2, Name = "Warszawa", Address = "Marszałkowska 1", City = "Warszawa", PostalCode = "00-001" };
        var facultyLodz = new Faculty { Id = 1, Name = "Informatyka Łódź", Code = "WIL", Location = lodz };
        var facultyWarsaw = new Faculty { Id = 2, Name = "Informatyka Warszawa", Code = "WIW", Location = warsaw };
        var itLodz = new FieldOfStudy { Id = 1, Name = "Informatyka", Code = "INF-L", Faculty = facultyLodz, DurationSemesters = 7 };
        var itWarsaw = new FieldOfStudy { Id = 2, Name = "Informatyka", Code = "INF-W", Faculty = facultyWarsaw, DurationSemesters = 7 };

        db.Courses.AddRange(
            new Course { Id = 1, Name = "Programowanie", Code = "PRG", Semester = 1, FieldOfStudy = itLodz },
            new Course { Id = 2, Name = "Bazy danych", Code = "BD", Semester = 2, FieldOfStudy = itLodz },
            new Course { Id = 3, Name = "Programowanie", Code = "PRG", Semester = 1, FieldOfStudy = itWarsaw });
        db.SaveChanges();
        return db;
    }

    private static CreateSyllabusDto NewSyllabus(int courseId, string year = "2026/2027") => new()
    {
        CourseId = courseId,
        AcademicYear = year,
        EctsPoints = 5,
        LectureHours = 30,
        ExerciseHours = 0,
        LaboratoryHours = 30,
        Description = "Podstawy programowania w C#",
        CourseObjectives = "Nauka programowania",
        AssessmentMethod = "Egzamin",
        Prerequisites = "Brak",
    };

    private static UpdateSyllabusDto ToUpdate(CreateSyllabusDto dto, string change) => new()
    {
        CourseId = dto.CourseId,
        AcademicYear = dto.AcademicYear,
        EctsPoints = dto.EctsPoints,
        LectureHours = dto.LectureHours,
        ExerciseHours = dto.ExerciseHours,
        LaboratoryHours = dto.LaboratoryHours,
        Description = dto.Description,
        CourseObjectives = dto.CourseObjectives,
        AssessmentMethod = dto.AssessmentMethod,
        Prerequisites = dto.Prerequisites,
        ChangeDescription = change,
    };

    [Fact]
    public async Task CreateAsync_CreatesFirstVersion()
    {
        using var db = CreateDb();
        var service = new SyllabusService(db);

        var result = await service.CreateAsync(NewSyllabus(1), LecturerId);

        Assert.True(result.Success);
        Assert.Equal(1, result.Data!.CurrentVersion);
        var versions = (await service.GetVersionsAsync(result.Data.Id))!.ToList();
        Assert.Single(versions);
        Assert.Equal("Utworzenie sylabusa", versions[0].ChangeDescription);
    }

    [Fact]
    public async Task UpdateAsync_AddsNewVersion_AndKeepsOldValuesInHistory()
    {
        using var db = CreateDb();
        var service = new SyllabusService(db);
        var dto = NewSyllabus(1);
        var created = (await service.CreateAsync(dto, LecturerId)).Data!;

        var update = ToUpdate(dto, "Więcej godzin laboratoriów");
        update.LaboratoryHours = 45;
        var result = await service.UpdateAsync(created.Id, update, LecturerId);

        Assert.True(result.Success);
        Assert.Equal(2, result.Data!.CurrentVersion);
        var versions = (await service.GetVersionsAsync(created.Id))!.ToList();
        Assert.Equal([2, 1], versions.Select(v => v.VersionNumber));
        Assert.Equal(45, versions[0].LaboratoryHours);
        Assert.Equal(30, versions[1].LaboratoryHours);
        Assert.Equal("Więcej godzin laboratoriów", versions[0].ChangeDescription);
    }

    [Fact]
    public async Task CreateAsync_Fails_WhenCourseDoesNotExist()
    {
        using var db = CreateDb();
        var service = new SyllabusService(db);

        var result = await service.CreateAsync(NewSyllabus(999), LecturerId);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task GetAllAsync_FiltersByLocationYearAndText()
    {
        using var db = CreateDb();
        var service = new SyllabusService(db);
        await service.CreateAsync(NewSyllabus(1, "2026/2027"), LecturerId);
        await service.CreateAsync(NewSyllabus(2, "2026/2027"), LecturerId);
        await service.CreateAsync(NewSyllabus(3, "2026/2027"), LecturerId);
        await service.CreateAsync(NewSyllabus(1, "2025/2026"), LecturerId);

        var inWarsaw = await service.GetAllAsync(new SyllabusFilterDto { LocationId = 2 });
        var lodzThisYear = await service.GetAllAsync(new SyllabusFilterDto { LocationId = 1, AcademicYear = "2026/2027" });
        var byText = await service.GetAllAsync(new SyllabusFilterDto { Search = "bazy" });

        Assert.Single(inWarsaw);
        Assert.Equal(2, lodzThisYear.Count());
        Assert.Equal("BD", Assert.Single(byText).CourseCode);
    }

    [Fact]
    public async Task Export_CreatesPdfAndDocxFiles()
    {
        using var db = CreateDb();
        var service = new SyllabusService(db);
        var exporter = new SyllabusExportService(db);
        var created = (await service.CreateAsync(NewSyllabus(1), LecturerId)).Data!;

        var pdf = await exporter.ExportPdfAsync(created.Id);
        var docx = await exporter.ExportDocxAsync(created.Id);

        Assert.Equal("%PDF", System.Text.Encoding.ASCII.GetString(pdf!, 0, 4));
        Assert.Equal("PK", System.Text.Encoding.ASCII.GetString(docx!, 0, 2));
        Assert.Null(await exporter.ExportPdfAsync(999));
    }

    [Fact]
    public async Task GetCurriculumGridAsync_GroupsCoursesBySemester()
    {
        using var db = CreateDb();
        var service = new SyllabusService(db);
        await service.CreateAsync(NewSyllabus(1), LecturerId);

        var grid = await service.GetCurriculumGridAsync(1, "2026/2027");

        Assert.NotNull(grid);
        Assert.Equal([1, 2], grid.Semesters.Select(s => s.Semester));
        Assert.Equal(60, grid.Semesters[0].TotalHours);
        Assert.Null(grid.Semesters[1].Courses[0].SyllabusId);
        Assert.Equal(5, grid.TotalEcts);
    }
}
