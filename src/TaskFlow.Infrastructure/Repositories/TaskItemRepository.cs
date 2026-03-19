using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories;

public class TaskItemRepository(TaskFlowDbContext context) : ITaskItemRepository
{
    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        // Ignorar tareas borradas lógicamente
        return await context.Tasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(Guid projectId)
    {
        return await context
            .Tasks.Where(t => t.ProjectId == projectId && !t.IsDeleted)
            .OrderBy(t => t.Order)
            .ToListAsync();
    }

    public async Task<int> GetNextTaskOrderAsync(Guid projectId)
    {
        var maxOrder =
            await context.Tasks.Where(t => t.ProjectId == projectId).MaxAsync(t => (int?)t.Order)
            ?? 0;

        return maxOrder + 1;
    }

    public async Task AddAsync(TaskItem task) => await context.Tasks.AddAsync(task);

    public Task UpdateAsync(TaskItem task)
    {
        context.Tasks.Update(task);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TaskItem task)
    {
        context.Tasks.Remove(task);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
