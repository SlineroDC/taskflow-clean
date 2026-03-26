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

        // cambio de Draft a Active
        if (project.Status == ProjectStatus.Draft)
        {
            project.SetStatus(ProjectStatus.Active);
            await projectRepository.UpdateAsync(project);
        }

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

    public async Task UncompleteTaskAsync(Guid taskId)
    {
        var task =
            await taskRepository.GetByIdAsync(taskId)
            ?? throw new DomainException("Tarea no encontrada.");

        task.Uncomplete();

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

    public async Task ReorderTaskAsync(Guid taskId, int newOrder)
    {
        var task =
            await taskRepository.GetByIdAsync(taskId)
            ?? throw new DomainException("Tarea no encontrada.");

        await ReorderTaskAsync(task.ProjectId, taskId, newOrder);
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

    public async Task UpdateTaskAsync(Guid taskId, string title, int priorityValue)
    {
        var task =
            await taskRepository.GetByIdAsync(taskId)
            ?? throw new DomainException("Tarea no encontrada.");

        task.UpdateDetails(title, (TaskPriority)priorityValue);

        await taskRepository.UpdateAsync(task);
        await taskRepository.SaveChangesAsync();
    }

    public async Task CompleteAllTasksAsync(Guid projectId)
    {
        var tasks = (await taskRepository.GetTasksByProjectIdAsync(projectId)).ToList();

        if (!tasks.Any())
            throw new DomainException("No hay tareas para completar en este proyecto.");

        foreach (var task in tasks.Where(t => !t.IsCompleted))
        {
            task.Complete();
        }

        await taskRepository.SaveChangesAsync();
    }
}
