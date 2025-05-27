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
    {
        return RedirectToAction("Login", "Account");
    }

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

    // Pobierz wszystkie username, których potrzebujesz do profili
    var usernames = swaps
        .SelectMany(s => new[] { s.TargetBook.User.Username, s.OfferedBook.User.Username })
        .Distinct()
        .ToList();

    // Pobierz profile na podstawie username
    var profiles = await _db.UserProfiles
        .Where(p => usernames.Contains(p.Username))
        .ToListAsync();

    // Zrób słownik username -> UserProfile dla łatwego dostępu
    var profileDict = profiles.ToDictionary(p => p.Username);

    // Przekaż do widoku swapy i słownik profili
    ViewData["CurrentUserId"] = userId;
    ViewData["Profiles"] = profileDict;

    return View(swaps);
}


    }
}

