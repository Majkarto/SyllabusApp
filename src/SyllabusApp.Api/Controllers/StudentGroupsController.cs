using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyllabusApp.Application.Dtos.StudentGroups;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Domain.Constants;

namespace SyllabusApp.Api.Controllers;

[ApiController]
[Route("student-groups")]
[Authorize]
public class StudentGroupsController : ControllerBase
{
    private readonly IStudentGroupService _studentGroupService;

    public StudentGroupsController(IStudentGroupService studentGroupService)
    {
        _studentGroupService = studentGroupService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var studentGroups = await _studentGroupService.GetAllAsync();
        return Ok(studentGroups);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var studentGroup = await _studentGroupService.GetByIdAsync(id);
        if (studentGroup == null)
        {
            return NotFound(new { message = $"Nie znaleziono grupy studenckiej o id {id}." });
        }
        return Ok(studentGroup);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Create([FromBody] CreateStudentGroupDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _studentGroupService.CreateAsync(dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                StudentGroupError.FieldOfStudyNotFound => BadRequest(new { message = result.ErrorMessage }),
                StudentGroupError.NameAlreadyExists => Conflict(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentGroupDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _studentGroupService.UpdateAsync(id, dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                StudentGroupError.NotFound => NotFound(new { message = result.ErrorMessage }),
                StudentGroupError.FieldOfStudyNotFound => BadRequest(new { message = result.ErrorMessage }),
                StudentGroupError.NameAlreadyExists => Conflict(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _studentGroupService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Nie znaleziono grupy studenckiej o id {id}." });
        }
        return NoContent();
    }
}
