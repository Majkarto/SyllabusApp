using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyllabusApp.Application.Dtos.Faculties;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Domain.Constants;

namespace SyllabusApp.Api.Controllers;

[ApiController]
[Route("faculties")]
[Authorize]
public class FacultiesController : ControllerBase
{
    private readonly IFacultyService _facultyService;

    public FacultiesController(IFacultyService facultyService)
    {
        _facultyService = facultyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var faculties = await _facultyService.GetAllAsync();
        return Ok(faculties);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var faculty = await _facultyService.GetByIdAsync(id);
        if (faculty == null)
        {
            return NotFound(new { message = $"Nie znaleziono wydziału o id {id}." });
        }
        return Ok(faculty);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Create([FromBody] CreateFacultyDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _facultyService.CreateAsync(dto);
        
        if (!result.Success)
        {
            return result.Error switch
            {
                FacultyError.LocationNotFound => BadRequest(new { message = result.ErrorMessage }),
                FacultyError.CodeAlreadyExists => Conflict(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFacultyDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _facultyService.UpdateAsync(id, dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                FacultyError.NotFound => NotFound(new { message = result.ErrorMessage }),
                FacultyError.LocationNotFound => BadRequest(new { message = result.ErrorMessage }),
                FacultyError.CodeAlreadyExists => Conflict(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _facultyService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Nie znaleziono wydziału o id {id}." });
        }
        return NoContent();
    }
}