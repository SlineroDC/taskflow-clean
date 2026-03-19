using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.WebMvc.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    [HttpGet]
    public IActionResult Login()
    {
        // Si el usuario ya está logueado, lo mandamos al Dashboard
        if (User.Identity is { IsAuthenticated: true })
            return RedirectToAction("Index", "Projects");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var user = await _authService.LoginAsync(dto);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Email, user.Email),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

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
    public IActionResult Register()
    {
        // Si el usuario ya está logueado, lo mandamos al Dashboard
        if (User.Identity is { IsAuthenticated: true })
            return RedirectToAction("Index", "Projects");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // Validación de confirmación de contraseña
        if (dto.Password != dto.ConfirmPassword)
        {
            TempData["ErrorMessage"] = "Las contraseñas no coinciden.";
            return View();
        }

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
