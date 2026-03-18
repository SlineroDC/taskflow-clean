using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;

public class Project
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ProjectStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();
    private readonly List<TaskItem> _tasks = [];

    protected Project() { } // Para EF Core

    public Project(string name, string description, Guid userId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        UserId = userId;
        Status = ProjectStatus.Draft;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    // Métodos de comportamiento del dominio
    public void UpdateDetails(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void Activate() => Status = ProjectStatus.Active;

    public void Complete() => Status = ProjectStatus.Completed;

    public void Delete()
    {
        IsDeleted = true;
    }
}
