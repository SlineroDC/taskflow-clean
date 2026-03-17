using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces;

public interface IProjectService
{
    Task<ProjectDto> CreateProjectAsync(Guid userId, string name, string description);
    Task<IEnumerable<ProjectDto>> GetProjectsAsync(Guid userId);
    Task ActivateProjectAsync(Guid projectId);
    Task CompleteProjectAsync(Guid projectId);
    Task DeleteProjectAsync(Guid projectId); // Este hará el Soft Delete
    Task<ProjectSummaryDto> GetProjectSummaryAsync(Guid projectId);
}
