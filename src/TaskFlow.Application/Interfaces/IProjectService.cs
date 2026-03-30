using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces;

public interface IProjectService
{
    Task<ProjectDto> CreateProjectAsync(Guid userId, CreateProjectDto dto);
    Task<IEnumerable<ProjectDto>> GetProjectsAsync(Guid userId);
    Task<ProjectDetailsDto> GetProjectDetailsAsync(Guid projectId);
    Task UpdateProjectAsync(Guid projectId, UpdateProjectDto dto);
    Task ActivateProjectAsync(Guid projectId);
    Task CompleteProjectAsync(Guid projectId);
    Task DeleteProjectAsync(Guid projectId);
}
