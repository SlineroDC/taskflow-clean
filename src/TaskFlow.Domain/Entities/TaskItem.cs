using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public TaskPriority Priority { get; private set; }
    public int Order { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsDeleted { get; private set; } // Para el Soft Delete
    public DateTime CreatedAt { get; private set; }

    // Navegación
    public Project? Project { get; private set; }

    protected TaskItem() { }

    public TaskItem(Guid projectId, string title, TaskPriority priority, int order)
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
        Title = title;
        Priority = priority;
        Order = order;
        IsCompleted = false;
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new DomainException("La tarea ya está completada.");
        IsCompleted = true;
    }

    public void Uncomplete()
    {
        if (!IsCompleted)
            throw new DomainException("La tarea ya está pendiente.");
        IsCompleted = false;
    }

    public void UpdateDetails(string title, TaskPriority priority)
    {
        Title = title;
        Priority = priority;
    }

    public void UpdateOrder(int newOrder)
    {
        Order = newOrder;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
