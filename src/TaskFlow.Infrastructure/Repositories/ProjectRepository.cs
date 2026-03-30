using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories;

public class ProjectRepository(TaskFlowDbContext context) : IProjectRepository
{
    public async Task<Project?> GetByIdAsync(Guid id, bool includeTasks = false)
    {
        var query = context.Projects.AsQueryable();

        if (includeTasks)
        {
            // Solo incluimos tareas que no estén eliminadas lógicamente
            query = query.Include(p => p.Tasks.Where(t => !t.IsDeleted)).AsSplitQuery();
        }

        return await query.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Project>> GetAllByUserIdAsync(Guid userId)
    {
        return await context
            .Projects.Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Project project) => await context.Projects.AddAsync(project);

    public Task UpdateAsync(Project project)
    {
        context.Projects.Update(project);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();

    public async Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId)
    {
        return await context
            .Projects.Include(p => p.Tasks)
            .Where(p => p.UserId == userId && !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
}
