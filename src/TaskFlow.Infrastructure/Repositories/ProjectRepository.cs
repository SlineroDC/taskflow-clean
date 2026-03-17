using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly TaskFlowDbContext _context;

    public ProjectRepository(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetByIdAsync(Guid id, bool includeTasks = false)
    {
        var query = _context.Projects.AsQueryable();

        if (includeTasks)
        {
            // Solo incluimos tareas que no estén eliminadas lógicamente
            query = query.Include(p => p.Tasks.Where(t => !t.IsDeleted)).AsSplitQuery();
        }

        return await query.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Project>> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Projects
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Project project) => await _context.Projects.AddAsync(project);

    public Task UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
