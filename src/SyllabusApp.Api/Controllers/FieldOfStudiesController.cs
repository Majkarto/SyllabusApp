using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyllabusApp.Application.Dtos.FieldOfStudies;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Domain.Constants;

namespace SyllabusApp.Api.Controllers;

[ApiController]
[Route("field-of-studies")]
[Authorize]
public class FieldOfStudiesController : ControllerBase
{
    private readonly IFieldOfStudyService _fieldOfStudyService;

    public FieldOfStudiesController(IFieldOfStudyService fieldOfStudyService)
    {
        _fieldOfStudyService = fieldOfStudyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var fieldsOfStudy = await _fieldOfStudyService.GetAllAsync();
        return Ok(fieldsOfStudy);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var fieldOfStudy = await _fieldOfStudyService.GetByIdAsync(id);
        if (fieldOfStudy == null)
        {
            return NotFound(new { message = $"Nie znaleziono kierunku studiów o id {id}." });
        }
        return Ok(fieldOfStudy);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Create([FromBody] CreateFieldOfStudyDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _fieldOfStudyService.CreateAsync(dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                FieldOfStudyError.FacultyNotFound => BadRequest(new { message = result.ErrorMessage }),
                FieldOfStudyError.CodeAlreadyExists => Conflict(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFieldOfStudyDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _fieldOfStudyService.UpdateAsync(id, dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                FieldOfStudyError.NotFound => NotFound(new { message = result.ErrorMessage }),
                FieldOfStudyError.FacultyNotFound => BadRequest(new { message = result.ErrorMessage }),
                FieldOfStudyError.CodeAlreadyExists => Conflict(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _fieldOfStudyService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Nie znaleziono kierunku studiów o id {id}." });
        }
        return NoContent();
    }
}
