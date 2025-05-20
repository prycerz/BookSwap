using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookSwap.Models;

namespace BookSwap.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

 public IActionResult Index()
{
    if (HttpContext.Session.GetString("username") == null)
    {
        // Jeśli brak sesji (niezalogowany), przekieruj na stronę logowania
        return RedirectToAction("Login", "Account");
    }

    // Jeśli zalogowany, pokaż widok Index
    return View();
}


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
