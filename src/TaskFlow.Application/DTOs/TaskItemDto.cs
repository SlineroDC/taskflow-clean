namespace TaskFlow.Application.DTOs;

public class AddTaskDto
{
    public string Title { get; set; } = string.Empty;
    public int PriorityValue { get; set; }
}

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsCompleted { get; set; }
}
