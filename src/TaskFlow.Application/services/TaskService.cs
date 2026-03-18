using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Services;

public class TaskService(ITaskItemRepository taskRepository, IProjectRepository projectRepository)
    : ITaskService
{
    public async Task<TaskItemDto> AddTaskAsync(Guid projectId, AddTaskDto dto)
    {
        var project =
            await projectRepository.GetByIdAsync(projectId)
            ?? throw new DomainException("Proyecto no encontrado.");

        if (project.Status == ProjectStatus.Completed)
            throw new DomainException("No se pueden agregar tareas a un proyecto completado.");

        int nextOrder = await taskRepository.GetNextTaskOrderAsync(projectId);
        var priority = (TaskPriority)dto.PriorityValue;

        var task = new TaskItem(projectId, dto.Title, priority, nextOrder);

        await taskRepository.AddAsync(task);
        await taskRepository.SaveChangesAsync();

        return new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Priority = task.Priority.ToString(),
            Order = task.Order,
            IsCompleted = task.IsCompleted,
        };
    }

    public async Task CompleteTaskAsync(Guid taskId)
    {
        var task =
            await taskRepository.GetByIdAsync(taskId)
            ?? throw new DomainException("Tarea no encontrada.");

        task.Complete();

        await taskRepository.SaveChangesAsync();
    }

    public async Task ReorderTaskAsync(Guid projectId, Guid taskId, int newOrder)
    {
        if (newOrder < 1)
            throw new DomainException("El orden debe ser mayor a cero.");

        var tasks = (await taskRepository.GetTasksByProjectIdAsync(projectId)).ToList();
        var taskToMove =
            tasks.FirstOrDefault(t => t.Id == taskId)
            ?? throw new DomainException("Tarea no encontrada en este proyecto.");

        int oldOrder = taskToMove.Order;
        if (oldOrder == newOrder)
            return;

        if (newOrder > oldOrder)
        {
            foreach (var t in tasks.Where(t => t.Order > oldOrder && t.Order <= newOrder))
            {
                t.UpdateOrder(t.Order - 1);
            }
        }
        else
        {
            foreach (var t in tasks.Where(t => t.Order >= newOrder && t.Order < oldOrder))
            {
                t.UpdateOrder(t.Order + 1);
            }
        }

        taskToMove.UpdateOrder(newOrder);

        // EF Core detecta todos los cambios y los guarda en una sola transacción
        await taskRepository.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(Guid taskId)
    {
        var task =
            await taskRepository.GetByIdAsync(taskId)
            ?? throw new DomainException("Tarea no encontrada.");

        task.SoftDelete();
        // Reajustar el orden de las tareas que estaban debajo
        var remainingTasks = (await taskRepository.GetTasksByProjectIdAsync(task.ProjectId))
            .Where(t => t.Order > task.Order)
            .ToList();

        foreach (var t in remainingTasks)
        {
            t.UpdateOrder(t.Order - 1);
        }

        await taskRepository.SaveChangesAsync();
    }
}
