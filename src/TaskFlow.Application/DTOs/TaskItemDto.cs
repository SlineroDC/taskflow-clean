namespace TaskFlow.Application.DTOs;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsCompleted { get; set; }
}
