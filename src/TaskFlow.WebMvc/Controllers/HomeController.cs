using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.WebMvc.Controllers;

public class HomeController : Controller
{
    private readonly IAuthService _authService;

    public HomeController(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Index() => View();

    public IActionResult Error(int statusCode)
    {
        ViewBag.StatusCode = statusCode;
        return View();
    }

    [Authorize]
    [HttpGet]
    public IActionResult Settings() => View();

    public IActionResult Docs() => View();

    public IActionResult Community() => View();

    public IActionResult Terms() => View();

    public IActionResult Security() => View();

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _authService.UpdateProfileAsync(userId, dto);

            var email = User.FindFirstValue(ClaimTypes.Email);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, dto.FullName),
                new Claim(ClaimTypes.Email, email!),
                new Claim("AvatarUrl", dto.Avatar),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            TempData["SuccessMessage"] = "Perfil actualizado correctamente.";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Settings));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _authService.ChangePasswordAsync(userId, dto);

            TempData["SuccessMessage"] = "Contraseña de seguridad actualizada con éxito.";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Settings));
    }
}
