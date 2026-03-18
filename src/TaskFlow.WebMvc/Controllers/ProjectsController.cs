using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.WebMvc.Controllers;

public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;

    // Inyectamos nuestro caso de uso (Aplicación) directamente en el controlador
    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // GET: /Projects (Esta es la ruta por defecto que configuramos en Program.cs)
    public async Task<IActionResult> Index()
    {
        // Como aún no tenemos login, simulamos un UserId fijo para probar
        var dummyUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var projects = await _projectService.GetProjectsAsync(dummyUserId);

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
            // Atrapamos las reglas de negocio (ej: "No se puede activar sin tareas")
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
