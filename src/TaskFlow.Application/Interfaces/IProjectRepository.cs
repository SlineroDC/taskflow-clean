using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id, bool includeTasks = false);
    Task<IEnumerable<Project>> GetAllByUserIdAsync(Guid userId);
    Task AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task SaveChangesAsync();
}
