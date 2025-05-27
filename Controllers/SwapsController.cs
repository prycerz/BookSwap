using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSwap.Models;

namespace BookSwap.Controllers
{
    public class SwapsController : Controller
    {
        private readonly AppDbContext _db;

        public SwapsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
{
    var username = HttpContext.Session.GetString("username");
    if (string.IsNullOrEmpty(username))
        return RedirectToAction("Login", "Account");

    var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
    if (user == null)
    {
        HttpContext.Session.Remove("username");
        return RedirectToAction("Login", "Account");
    }

    int userId = user.Id;

    var swaps = await _db.SwapRequests
        .Include(s => s.TargetBook)
        .ThenInclude(b => b.User)
        .Include(s => s.OfferedBook)
        .ThenInclude(b => b.User)
        .Where(s => s.TargetBookOwnerId == userId || s.OfferedBookOwnerId == userId)
        .OrderByDescending(s => s.CreatedAt)
        .ToListAsync();

    // Zamień miejscami książki i właścicieli jeśli aktualny user nie jest oferującym
    foreach (var swap in swaps)
    {
        if (swap.OfferedBookOwnerId != userId)
        {
            // Zamiana książek
            var tempBook = swap.OfferedBook;
            swap.OfferedBook = swap.TargetBook;
            swap.TargetBook = tempBook;

            // Zamiana właścicieli
            var tempOwnerId = swap.OfferedBookOwnerId;
            swap.OfferedBookOwnerId = swap.TargetBookOwnerId;
            swap.TargetBookOwnerId = tempOwnerId;
        }
    }

    return View(swaps);
}

    }
}

