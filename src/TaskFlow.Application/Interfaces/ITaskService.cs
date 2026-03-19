using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces;

public interface ITaskService
{
    Task<TaskItemDto> AddTaskAsync(Guid projectId, AddTaskDto dto);
    Task CompleteTaskAsync(Guid taskId);
    Task ReorderTaskAsync(Guid projectId, Guid taskId, int newOrder);
    Task ReorderTaskAsync(Guid taskId, int newOrder);
    Task DeleteTaskAsync(Guid taskId);
    Task UpdateTaskAsync(Guid taskId, string title, int priorityValue);
    Task CompleteAllTasksAsync(Guid projectId);
}
