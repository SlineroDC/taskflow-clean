using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities;

public class Project
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ProjectStatus Status { get; private set; }
    public bool IsDeleted { get; private set; } // Para el test DeleteProject_ShouldBeDelete
    public DateTime CreatedAt { get; private set; }

    // Navegación encapsulada
    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();
    private readonly List<TaskItem> _tasks = new();

    public User? User { get; private set; }

    protected Project() { }

    public Project(Guid userId, string name, string description)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Name = name;
        Description = description;
        Status = ProjectStatus.Draft;
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    // --- REGLAS DE NEGOCIO OBLIGATORIAS ---

    public void Activate()
    {
        if (Status != ProjectStatus.Draft)
            throw new DomainException("Solo los proyectos en estado Draft pueden activarse.");

        // REGLA: Un proyecto solo puede activarse si tiene al menos una tarea.
        if (!_tasks.Any(t => !t.IsDeleted))
            throw new DomainException(
                "El proyecto debe tener al menos una tarea para ser activado."
            );

        Status = ProjectStatus.Active;
    }

    public void Complete()
    {
        if (Status != ProjectStatus.Active)
            throw new DomainException(
                "Solo los proyectos activos pueden marcarse como completados."
            );

        // REGLA: Un proyecto solo puede completarse si todas sus tareas están completadas.
        if (_tasks.Any(t => !t.IsDeleted && !t.IsCompleted))
            throw new DomainException(
                "No se puede completar el proyecto porque hay tareas pendientes."
            );

        Status = ProjectStatus.Completed;
    }

    // --- MÉTODOS DE COMPORTAMIENTO ---

    public void AddTask(TaskItem task)
    {
        _tasks.Add(task);
    }

    public void UpdateDetails(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
