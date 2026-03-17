using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;

    public TaskService(ITaskItemRepository taskRepository, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
    }

    public async Task<TaskItemDto> AddTaskAsync(Guid projectId, string title, int priorityValue)
    {
        // Validar que el proyecto exista y no esté completado
        var project =
            await _projectRepository.GetByIdAsync(projectId)
            ?? throw new DomainException("Proyecto no encontrado.");

        if (project.Status == ProjectStatus.Completed)
            throw new DomainException("No se pueden agregar tareas a un proyecto completado.");

        // Generar el siguiente orden disponible automáticamente
        int nextOrder = await _taskRepository.GetNextTaskOrderAsync(projectId);
        var priority = (TaskPriority)priorityValue;

        var task = new TaskItem(projectId, title, priority, nextOrder);

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();

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
            await _taskRepository.GetByIdAsync(taskId)
            ?? throw new DomainException("Tarea no encontrada.");

        task.Complete();

        await _taskRepository.UpdateAsync(task);
        await _taskRepository.SaveChangesAsync();
    }

    public async Task ReorderTaskAsync(Guid projectId, Guid taskId, int newOrder)
    {
        if (newOrder < 1)
            throw new DomainException("El orden debe ser mayor a cero.");

        var tasks = (await _taskRepository.GetTasksByProjectIdAsync(projectId)).ToList();
        var taskToMove =
            tasks.FirstOrDefault(t => t.Id == taskId)
            ?? throw new DomainException("Tarea no encontrada en este proyecto.");

        int oldOrder = taskToMove.Order;
        if (oldOrder == newOrder)
            return; // No hay cambios

        // LÓGICA DE DESPLAZAMIENTO (SHIFT) PARA EVITAR DUPLICADOS
        if (newOrder > oldOrder)
        {
            var affectedTasks = tasks.Where(t => t.Order > oldOrder && t.Order <= newOrder);
            foreach (var t in affectedTasks)
            {
                t.UpdateOrder(t.Order - 1);
                await _taskRepository.UpdateAsync(t);
            }
        }
        else
        {
            var affectedTasks = tasks.Where(t => t.Order >= newOrder && t.Order < oldOrder);
            foreach (var t in affectedTasks)
            {
                t.UpdateOrder(t.Order + 1);
                await _taskRepository.UpdateAsync(t);
            }
        }

        taskToMove.UpdateOrder(newOrder);
        await _taskRepository.UpdateAsync(taskToMove);

        // Se guarda todo en una sola transacción para garantizar la integridad
        await _taskRepository.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(Guid taskId)
    {
        var task =
            await _taskRepository.GetByIdAsync(taskId)
            ?? throw new DomainException("Tarea no encontrada.");

        task.SoftDelete();
        var remainingTasks = (await _taskRepository.GetTasksByProjectIdAsync(task.ProjectId))
            .Where(t => t.Order > task.Order)
            .ToList();

        foreach (var t in remainingTasks)
        {
            t.UpdateOrder(t.Order - 1);
            await _taskRepository.UpdateAsync(t);
        }

        await _taskRepository.UpdateAsync(task);
        await _taskRepository.SaveChangesAsync();
    }
}
