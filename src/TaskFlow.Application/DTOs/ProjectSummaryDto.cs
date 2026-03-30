namespace TaskFlow.Application.DTOs;

public class ProjectSummaryDto
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public decimal CompletionPercentage =>
        TotalTasks == 0 ? 0 : Math.Round((decimal)CompletedTasks / TotalTasks * 100, 2);
}
