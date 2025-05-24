using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookSwap.Models;
using Microsoft.EntityFrameworkCore;

namespace BookSwap.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
    {
        if (HttpContext.Session.GetString("username") == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var books = await _context.Books
            .Include(b => b.User)
            .OrderByDescending(b => b.DateAdded)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var usernames = books.Select(b => b.User.Username).Distinct().ToList();

        var profiles = await _context.UserProfiles
            .Where(p => usernames.Contains(p.Username))
            .ToListAsync();

        var profileDict = profiles.ToDictionary(p => p.Username);

        var bookViewModels = books.Select(book => new BookWithProfileViewModel
        {
            Book = book,
            Profile = profileDict.TryGetValue(book.User.Username, out var profile) ? profile : null
        }).ToList();

        return View(bookViewModels);
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


