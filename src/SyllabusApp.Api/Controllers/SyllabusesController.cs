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

    public SyllabusesController(ISyllabusService syllabusService)
    {
        _syllabusService = syllabusService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var syllabuses = await _syllabusService.GetAllAsync();
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

        var result = await _syllabusService.UpdateAsync(id, dto);

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
