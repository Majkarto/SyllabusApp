using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyllabusApp.Application.Dtos.Syllabuses;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Domain.Constants;

namespace SyllabusApp.Api.Controllers;

[ApiController]
[Route("syllabuses")]
[Authorize]
public class SyllabusesController : ControllerBase
{
    private readonly ISyllabusService _syllabusService;
    private readonly ISyllabusExportService _exportService;

    public SyllabusesController(ISyllabusService syllabusService, ISyllabusExportService exportService)
    {
        _syllabusService = syllabusService;
        _exportService = exportService;
    }

    [HttpGet("{id:int}/versions")]
    public async Task<IActionResult> GetVersions(int id)
    {
        var versions = await _syllabusService.GetVersionsAsync(id);
        if (versions == null)
        {
            return NotFound(new { message = $"Nie znaleziono sylabusa o id {id}." });
        }
        return Ok(versions);
    }

    [HttpGet("curriculum-grid")]
    public async Task<IActionResult> GetCurriculumGrid([FromQuery] int fieldOfStudyId, [FromQuery] string? academicYear)
    {
        var grid = await _syllabusService.GetCurriculumGridAsync(fieldOfStudyId, academicYear);
        if (grid == null)
        {
            return NotFound(new { message = $"Nie znaleziono kierunku o id {fieldOfStudyId}." });
        }
        return Ok(grid);
    }

    [HttpGet("{id:int}/export/pdf")]
    public async Task<IActionResult> ExportPdf(int id)
    {
        var bytes = await _exportService.ExportPdfAsync(id);
        return bytes == null
            ? NotFound(new { message = $"Nie znaleziono sylabusa o id {id}." })
            : File(bytes, "application/pdf", $"sylabus-{id}.pdf");
    }

    [HttpGet("{id:int}/export/docx")]
    public async Task<IActionResult> ExportDocx(int id)
    {
        var bytes = await _exportService.ExportDocxAsync(id);
        return bytes == null
            ? NotFound(new { message = $"Nie znaleziono sylabusa o id {id}." })
            : File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"sylabus-{id}.docx");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SyllabusFilterDto filter)
    {
        var syllabuses = await _syllabusService.GetAllAsync(filter);
        return Ok(syllabuses);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var syllabus = await _syllabusService.GetByIdAsync(id);
        if (syllabus == null)
        {
            return NotFound(new { message = $"Nie znaleziono sylabusa o id {id}." });
        }
        return Ok(syllabus);
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Lecturer}")]
    public async Task<IActionResult> Create([FromBody] CreateSyllabusDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var lecturerId))
        {
            return Unauthorized(new { message = "Nie udało się zidentyfikować zalogowanego użytkownika." });
        }

        var result = await _syllabusService.CreateAsync(dto, lecturerId);

        if (!result.Success)
        {
            return result.Error switch
            {
                SyllabusError.CourseNotFound => BadRequest(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Lecturer}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSyllabusDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int? userId = int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsed) ? parsed : null;
        var result = await _syllabusService.UpdateAsync(id, dto, userId);

        if (!result.Success)
        {
            return result.Error switch
            {
                SyllabusError.NotFound => NotFound(new { message = result.ErrorMessage }),
                SyllabusError.CourseNotFound => BadRequest(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Lecturer}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _syllabusService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Nie znaleziono sylabusa o id {id}." });
        }
        return NoContent();
    }
}
