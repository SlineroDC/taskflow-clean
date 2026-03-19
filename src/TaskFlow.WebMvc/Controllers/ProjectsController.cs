using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.WebMvc.Controllers;

[Authorize]
public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // --- HELPER PRIVADO PARA OBTENER EL USUARIO (DRY) ---
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("Usuario no autenticado.");

        return Guid.Parse(userIdClaim);
    }

    // GET: /Projects
    public async Task<IActionResult> Index()
    {
        try
        {
            var userId = GetCurrentUserId();
            var projects = await _projectService.GetProjectsAsync(userId);
            return View(projects);
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("Login", "Auth");
        }
    }

    // POST: /Projects/Create
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto dto)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Index));

        try
        {
            var userId = GetCurrentUserId();
            await _projectService.CreateProjectAsync(userId, dto);
            TempData["SuccessMessage"] = "Proyecto inicializado correctamente.";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("Login", "Auth");
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: /Projects/Edit/{id}
    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, UpdateProjectDto dto)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Index));

        try
        {
            await _projectService.UpdateProjectAsync(id, dto);
            TempData["SuccessMessage"] = "Proyecto actualizado correctamente.";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: /Projects/Details/{id}
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var project = await _projectService.GetProjectDetailsAsync(id);
            return View(project);
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: /Projects/Activate/{id}
    [HttpPost]
    public async Task<IActionResult> Activate(Guid id)
    {
        try
        {
            await _projectService.ActivateProjectAsync(id);
            TempData["SuccessMessage"] = "Proyecto activado correctamente.";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: /Projects/Delete/{id}
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _projectService.DeleteProjectAsync(id);
            TempData["SuccessMessage"] = "Proyecto eliminado del sistema.";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Complete(Guid id)
    {
        try
        {
            await _projectService.CompleteProjectAsync(id); // Tu servicio validará que no haya tareas pendientes
            TempData["SuccessMessage"] = "¡Felicidades! Sprint completado con éxito.";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }
}
