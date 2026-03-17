using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDto> CreateProjectAsync(Guid userId, string name, string description)
    {
        var project = new Project(userId, name, description);
        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Status = project.Status.ToString(),
            CreatedAt = project.CreatedAt,
        };
    }

    public async Task<IEnumerable<ProjectDto>> GetProjectsAsync(Guid userId)
    {
        var projects = await _projectRepository.GetAllByUserIdAsync(userId);
        return projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Status = p.Status.ToString(),
            CreatedAt = p.CreatedAt,
        });
    }

    public async Task ActivateProjectAsync(Guid projectId)
    {
        // exige verificar si hay tareas antes de activar.
        var project =
            await _projectRepository.GetByIdAsync(projectId, includeTasks: true)
            ?? throw new DomainException("Proyecto no encontrado.");

        project.Activate(); // La entidad valida la regla de negocio

        await _projectRepository.UpdateAsync(project);
        await _projectRepository.SaveChangesAsync();
    }

    public async Task CompleteProjectAsync(Guid projectId)
    {
        var project =
            await _projectRepository.GetByIdAsync(projectId, includeTasks: true)
            ?? throw new DomainException("Proyecto no encontrado.");

        project.Complete();

        await _projectRepository.UpdateAsync(project);
        await _projectRepository.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(Guid projectId)
    {
        var project =
            await _projectRepository.GetByIdAsync(projectId)
            ?? throw new DomainException("Proyecto no encontrado.");

        project.SoftDelete();

        await _projectRepository.UpdateAsync(project);
        await _projectRepository.SaveChangesAsync();
    }

    public async Task<ProjectSummaryDto> GetProjectSummaryAsync(Guid projectId)
    {
        var project =
            await _projectRepository.GetByIdAsync(projectId, includeTasks: true)
            ?? throw new DomainException("Proyecto no encontrado.");

        var activeTasks = project.Tasks.Where(t => !t.IsDeleted).ToList();

        return new ProjectSummaryDto
        {
            ProjectId = project.Id,
            Name = project.Name,
            Status = project.Status.ToString(),
            TotalTasks = activeTasks.Count,
            CompletedTasks = activeTasks.Count(t => t.IsCompleted),
        };
    }
}
