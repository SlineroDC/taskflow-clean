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

    public IActionResult Terms() => View();

    public IActionResult Security() => View();

    public IActionResult Docs() => View();
}
