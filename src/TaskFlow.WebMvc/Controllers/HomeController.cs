using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.WebMvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Error(int statusCode)
    {
        ViewBag.StatusCode = statusCode;
        return View();
    }

    [Authorize]
    public IActionResult Settings() => View();

    [Authorize]
    public IActionResult Docs() => View();

    [Authorize]
    public IActionResult Community() => View();

    public IActionResult Terms() => View();

    public IActionResult Security() => View();
}
