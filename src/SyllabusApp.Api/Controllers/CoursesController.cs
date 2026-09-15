using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyllabusApp.Application.Dtos.Courses;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Domain.Constants;

namespace SyllabusApp.Api.Controllers;

[ApiController]
[Route("courses")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _courseService.GetAllAsync();
        return Ok(courses);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var course = await _courseService.GetByIdAsync(id);
        if (course == null)
        {
            return NotFound(new { message = $"Nie znaleziono przedmiotu o id {id}." });
        }
        return Ok(course);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _courseService.CreateAsync(dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                CourseError.FieldOfStudyNotFound => BadRequest(new { message = result.ErrorMessage }),
                CourseError.CodeAlreadyExists => Conflict(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _courseService.UpdateAsync(id, dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                CourseError.NotFound => NotFound(new { message = result.ErrorMessage }),
                CourseError.FieldOfStudyNotFound => BadRequest(new { message = result.ErrorMessage }),
                CourseError.CodeAlreadyExists => Conflict(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _courseService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Nie znaleziono przedmiotu o id {id}." });
        }
        return NoContent();
    }
}
