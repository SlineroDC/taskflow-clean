using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.WebMvc.Controllers;

public class AuthController : Controller
{
    // GET: /Auth/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Auth/Login (Simulación para entrar al dashboard)
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        // Por ahora, cualquier login nos lleva al dashboard simulado
        return RedirectToAction("Index", "Projects");
    }

    // GET: /Auth/Register
    public IActionResult Register()
    {
        return View();
    }
}
