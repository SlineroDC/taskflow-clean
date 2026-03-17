using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly TaskFlowDbContext _context;

    public TaskItemRepository(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(Guid projectId)
    {
        return await _context
            .Tasks.Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.Order)
            .ToListAsync();
    }

    public async Task<int> GetNextTaskOrderAsync(Guid projectId)
    {
        // Calcula el siguiente orden disponible para una nueva tarea
        var maxOrder =
            await _context.Tasks.Where(t => t.ProjectId == projectId).MaxAsync(t => (int?)t.Order)
            ?? 0;

        return maxOrder + 1;
    }

    public async Task AddAsync(TaskItem task) => await _context.Tasks.AddAsync(task);

    public Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TaskItem task)
    {
        _context.Tasks.Remove(task);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
