using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyllabusApp.Application.Dtos.Literatures;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Domain.Constants;

namespace SyllabusApp.Api.Controllers;

[ApiController]
[Route("literatures")]
[Authorize]
public class LiteraturesController : ControllerBase
{
    private readonly ILiteratureService _literatureService;

    public LiteraturesController(ILiteratureService literatureService)
    {
        _literatureService = literatureService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var literature = await _literatureService.GetAllAsync();
        return Ok(literature);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var literature = await _literatureService.GetByIdAsync(id);
        if (literature == null)
        {
            return NotFound(new { message = $"Nie znaleziono pozycji literatury o id {id}." });
        }
        return Ok(literature);
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Lecturer}")]
    public async Task<IActionResult> Create([FromBody] CreateLiteratureDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _literatureService.CreateAsync(dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                LiteratureError.SyllabusNotFound => BadRequest(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Lecturer}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLiteratureDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _literatureService.UpdateAsync(id, dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                LiteratureError.NotFound => NotFound(new { message = result.ErrorMessage }),
                LiteratureError.SyllabusNotFound => BadRequest(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Lecturer}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _literatureService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Nie znaleziono pozycji literatury o id {id}." });
        }
        return NoContent();
    }
}
