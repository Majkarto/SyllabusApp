using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyllabusApp.Application.Dtos.WorkloadItems;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Domain.Constants;

namespace SyllabusApp.Api.Controllers;

[ApiController]
[Route("workload-items")]
[Authorize]
public class WorkloadItemsController : ControllerBase
{
    private readonly IWorkloadItemService _workloadItemService;

    public WorkloadItemsController(IWorkloadItemService workloadItemService)
    {
        _workloadItemService = workloadItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var workloadItems = await _workloadItemService.GetAllAsync();
        return Ok(workloadItems);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var workloadItem = await _workloadItemService.GetByIdAsync(id);
        if (workloadItem == null)
        {
            return NotFound(new { message = $"Nie znaleziono nakładu pracy o id {id}." });
        }
        return Ok(workloadItem);
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Lecturer}")]
    public async Task<IActionResult> Create([FromBody] CreateWorkloadItemDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _workloadItemService.CreateAsync(dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                WorkloadItemError.SyllabusNotFound => BadRequest(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Lecturer}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkloadItemDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _workloadItemService.UpdateAsync(id, dto);

        if (!result.Success)
        {
            return result.Error switch
            {
                WorkloadItemError.NotFound => NotFound(new { message = result.ErrorMessage }),
                WorkloadItemError.SyllabusNotFound => BadRequest(new { message = result.ErrorMessage }),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Lecturer}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _workloadItemService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Nie znaleziono nakładu pracy o id {id}." });
        }
        return NoContent();
    }
}
