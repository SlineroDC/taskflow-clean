using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.WebMvc.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var user = await _authService.LoginAsync(dto);

            // Crear los "Claims" (Datos que guardaremos en la Cookie)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            // Iniciar sesión en .NET (Genera la Cookie)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            return RedirectToAction("Index", "Projects");
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return View();
        }
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            await _authService.RegisterAsync(dto);
            TempData["SuccessMessage"] = "Cuenta creada con éxito. Ahora puedes iniciar sesión.";
            return RedirectToAction(nameof(Login));
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
