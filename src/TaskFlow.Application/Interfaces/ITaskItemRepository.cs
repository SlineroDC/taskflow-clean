using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

public interface ITaskItemRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(Guid projectId);
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
    
    // Método clave para calcular el orden automático al crear una nueva tarea
    Task<int> GetNextTaskOrderAsync(Guid projectId);
    Task SaveChangesAsync();
}
