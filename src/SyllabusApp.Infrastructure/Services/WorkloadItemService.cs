using Microsoft.EntityFrameworkCore;
using SyllabusApp.Application.Dtos.WorkloadItems;
using SyllabusApp.Application.Interfaces;
using SyllabusApp.Infrastructure.Persistence;

namespace SyllabusApp.Infrastructure.Services;

public class WorkloadItemService : IWorkloadItemService
{
    private readonly SyllabusDbContext _dbContext;

    public WorkloadItemService(SyllabusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<WorkloadItemDto>> GetAllAsync()
    {
        return await _dbContext.WorkloadItems
            .AsNoTracking()
            .Include(w => w.Syllabus)
            .OrderBy(w => w.Description)
            .Select(w => w.ToDto())
            .ToListAsync();
    }

    public async Task<WorkloadItemDto?> GetByIdAsync(int id)
    {
        var workloadItem = await _dbContext.WorkloadItems
            .AsNoTracking()
            .Include(w => w.Syllabus)
            .FirstOrDefaultAsync(w => w.Id == id);

        return workloadItem?.ToDto();
    }

    public async Task<WorkloadItemResult> CreateAsync(CreateWorkloadItemDto dto)
    {
        var syllabusExists = await _dbContext.Syllabuses.AnyAsync(s => s.Id == dto.SyllabusId);
        if (!syllabusExists)
        {
            return WorkloadItemResult.Fail(WorkloadItemError.SyllabusNotFound,
                $"Nie znaleziono sylabusa o id {dto.SyllabusId}.");
        }

        var workloadItem = dto.ToEntity();
        _dbContext.WorkloadItems.Add(workloadItem);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(workloadItem).Reference(w => w.Syllabus).LoadAsync();

        return WorkloadItemResult.Ok(workloadItem.ToDto());
    }

    public async Task<WorkloadItemResult> UpdateAsync(int id, UpdateWorkloadItemDto dto)
    {
        var workloadItem = await _dbContext.WorkloadItems.FirstOrDefaultAsync(w => w.Id == id);
        if (workloadItem == null)
        {
            return WorkloadItemResult.Fail(WorkloadItemError.NotFound,
                $"Nie znaleziono nakładu pracy o id {id}.");
        }

        var syllabusExists = await _dbContext.Syllabuses.AnyAsync(s => s.Id == dto.SyllabusId);
        if (!syllabusExists)
        {
            return WorkloadItemResult.Fail(WorkloadItemError.SyllabusNotFound,
                $"Nie znaleziono sylabusa o id {dto.SyllabusId}.");
        }

        workloadItem.ApplyUpdate(dto);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(workloadItem).Reference(w => w.Syllabus).LoadAsync();

        return WorkloadItemResult.Ok(workloadItem.ToDto());
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var workloadItem = await _dbContext.WorkloadItems.FirstOrDefaultAsync(w => w.Id == id);
        if (workloadItem == null)
        {
            return false;
        }

        _dbContext.WorkloadItems.Remove(workloadItem);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
