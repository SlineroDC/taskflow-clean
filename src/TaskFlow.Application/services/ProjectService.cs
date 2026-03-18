using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Services;

public class ProjectService(IProjectRepository projectRepository, IUserRepository userRepository)
    : IProjectService
{
    public async Task<ProjectDto> CreateProjectAsync(Guid userId, CreateProjectDto dto)
    {
        // 1. Validar que el usuario exista
        var user =
            await userRepository.GetByIdAsync(userId)
            ?? throw new DomainException("El usuario autenticado no existe en la base de datos.");

        // 2. Crear la entidad
        var project = new Project(dto.Name, dto.Description, userId);

        await projectRepository.AddAsync(project);
        await projectRepository.SaveChangesAsync();

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
        var projects = await projectRepository.GetByUserIdAsync(userId);

        return projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Status = p.Status.ToString(),
            CreatedAt = p.CreatedAt,
        });
    }

    public async Task<ProjectDetailsDto> GetProjectDetailsAsync(Guid projectId)
    {
        var project =
            await projectRepository.GetByIdAsync(projectId, includeTasks: true)
            ?? throw new DomainException("Proyecto no encontrado.");

        var activeTasks = project.Tasks.Where(t => !t.IsDeleted).ToList();

        return new ProjectDetailsDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Status = project.Status.ToString(),
            TotalTasks = activeTasks.Count,
            CompletedTasks = activeTasks.Count(t => t.IsCompleted),
            PendingTasks = activeTasks.Count(t => !t.IsCompleted),
            Tasks = activeTasks
                .Select(t => new TaskItemDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Priority = t.Priority.ToString(),
                    Order = t.Order,
                    IsCompleted = t.IsCompleted,
                })
                .OrderBy(t => t.Order)
                .ToList(),
        };
    }

    public async Task UpdateProjectAsync(Guid projectId, UpdateProjectDto dto)
    {
        var project =
            await projectRepository.GetByIdAsync(projectId)
            ?? throw new DomainException("Proyecto no encontrado.");

        project.UpdateDetails(dto.Name, dto.Description);

        await projectRepository.SaveChangesAsync();
    }

    public async Task ActivateProjectAsync(Guid projectId)
    {
        var project =
            await projectRepository.GetByIdAsync(projectId)
            ?? throw new DomainException("Proyecto no encontrado.");

        project.Activate();
        await projectRepository.SaveChangesAsync();
    }

    public async Task CompleteProjectAsync(Guid projectId)
    {
        var project =
            await projectRepository.GetByIdAsync(projectId, includeTasks: true)
            ?? throw new DomainException("Proyecto no encontrado.");

        project.Complete();
        await projectRepository.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(Guid projectId)
    {
        var project =
            await projectRepository.GetByIdAsync(projectId)
            ?? throw new DomainException("Proyecto no encontrado.");
        project.Delete();

        await projectRepository.SaveChangesAsync();
    }
}
