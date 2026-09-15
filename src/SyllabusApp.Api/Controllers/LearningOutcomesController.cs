using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyllabusApp.Application.Dtos.LearningOutcomes;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Domain.Constants;

namespace SyllabusApp.Api.Controllers;

[ApiController]
[Route("learning-outcomes")]
[Authorize]
public class LearningOutcomesController : ControllerBase
{
    private readonly ILearningOutcomeService _learningOutcomeService;

    public LearningOutcomesController(ILearningOutcomeService learningOutcomeService)
    {
        _learningOutcomeService = learningOutcomeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var learningOutcomes = await _learningOutcomeService.GetAllAsync();
        return Ok(learningOutcomes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var learningOutcome = await _learningOutcomeService.GetByIdAsync(id);
        if (learningOutcome == null)
        {
            return NotFound(new { message = $"Nie znaleziono efektu uczenia się o id {id}." });
        }
        return Ok(learningOutcome);
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Lecturer}")]
    public async Task<IActionResult> Create([FromBody] CreateLearningOutcomeDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _learningOutcomeService.CreateAsync(dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                LearningOutcomeError.SyllabusNotFound => BadRequest(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Lecturer}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLearningOutcomeDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _learningOutcomeService.UpdateAsync(id, dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                LearningOutcomeError.NotFound => NotFound(new { message = result.ErrorMessage }),
                LearningOutcomeError.SyllabusNotFound => BadRequest(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Lecturer}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _learningOutcomeService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Nie znaleziono efektu uczenia się o id {id}." });
        }
        return NoContent();
    }
}
