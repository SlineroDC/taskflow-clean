using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.WebMvc.Controllers;

[Authorize]
public class TasksController : Controller
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid projectId, AddTaskDto dto)
    {
        try
        {
            await _taskService.AddTaskAsync(projectId, dto);
            TempData["SuccessMessage"] = "Tarea añadida al Sprint.";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        // Redirigimos de vuelta a los detalles del proyecto
        return RedirectToAction("Details", "Projects", new { id = projectId });
    }

    [HttpPost]
    public async Task<IActionResult> Complete(Guid id, Guid projectId)
    {
        try
        {
            await _taskService.CompleteTaskAsync(id);
            TempData["SuccessMessage"] = "¡Tarea completada!";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction("Details", "Projects", new { id = projectId });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, Guid projectId)
    {
        try
        {
            await _taskService.DeleteTaskAsync(id);
            TempData["SuccessMessage"] = "Tarea eliminada del Backlog.";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction("Details", "Projects", new { id = projectId });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, Guid projectId, string title, int priorityValue)
    {
        try
        {
            await _taskService.UpdateTaskAsync(id, title, priorityValue);
            TempData["SuccessMessage"] = "Tarea actualizada correctamente.";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction("Details", "Projects", new { id = projectId });
    }

    [HttpPost]
    public async Task<IActionResult> Reorder(Guid id, int newOrder)
    {
        // Llama a tu servicio para actualizar el Order de la tarea
        await _taskService.ReorderTaskAsync(id, newOrder);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CompleteAll(Guid projectId)
    {
        // Lógica en tu servicio para buscar las tareas pendientes de este proyecto y marcarlas en true
         await _taskService.CompleteAllTasksAsync(projectId);
        TempData["SuccessMessage"] = "Todas las tareas fueron completadas.";
        return RedirectToAction("Details", "Projects", new { id = projectId });
    }
}
