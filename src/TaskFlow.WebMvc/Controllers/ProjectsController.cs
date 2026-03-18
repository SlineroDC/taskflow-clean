using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.WebMvc.Controllers;

// 1. EL CANDADO: Esto es lo que faltaba. Sin esto, la página es pública.
[Authorize]
public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // GET: /Projects
    public async Task<IActionResult> Index()
    {
        // 2. SEGURIDAD REAL: Obtenemos el ID del usuario logueado desde la Cookie
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim))
        {
            // Si por algún error de configuración no hay ID, cerramos sesión y mandamos al login
            return RedirectToAction("Login", "Auth");
        }

        var userId = Guid.Parse(userIdClaim);

        // Ahora pedimos los proyectos del usuario REAL, no del dummy
        var projects = await _projectService.GetProjectsAsync(userId);

        return View(projects);
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

    // GET: /Projects/Details/{id}
    // public async Task<IActionResult> Details(Guid id)
    // {
    //     try
    //     {
    //         // También protegemos los detalles para que solo se vean proyectos existentes
    //         var project = await _projectService.GetProjectDetailsAsync(id);
    //         return View(project);
    //     }
    //     catch (DomainException ex)
    //     {
    //         TempData["ErrorMessage"] = ex.Message;
    //         return RedirectToAction(nameof(Index));
    //     }
    // }
}
