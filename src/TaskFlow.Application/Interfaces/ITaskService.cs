using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces;

public interface ITaskService
{
    Task<TaskItemDto> AddTaskAsync(Guid projectId, string title, int priorityValue);
    Task CompleteTaskAsync(Guid taskId);
    Task ReorderTaskAsync(Guid projectId, Guid taskId, int newOrder); // Regla compleja
    Task DeleteTaskAsync(Guid taskId);
}
