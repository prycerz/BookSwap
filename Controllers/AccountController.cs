using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSwap.Models;
public class AccountController : Controller
{
    private readonly AppDbContext _db;

    public AccountController(AppDbContext db) => _db = db;

    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("username") != null)
            return RedirectToAction("Welcome");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (HttpContext.Session.GetString("username") != null)
            return RedirectToAction("Welcome");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            HttpContext.Session.SetString("username", username);
            HttpContext.Session.SetInt32("userId", user.Id);
            HttpContext.Session.SetString("role", user.Role);

            return RedirectToAction("Index", "Home");
        }
        ViewBag.Error = "Invalid credentials";
        return View();
    }

    public IActionResult Register()
    {
        var role = HttpContext.Session.GetString("role");
        if (role != "Admin")
            return RedirectToAction("Login"); // albo inna strona np. AccessDenied

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string username, string password)
    {
        var role = HttpContext.Session.GetString("role");
        if (role != "Admin")
            return RedirectToAction("Login");

        if (await _db.Users.AnyAsync(u => u.Username == username))
        {
            ViewBag.Error = "Username already taken";
            return View();
        }

        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "User" // lub inna domyślna rola
        };

        _db.Users.Add(user);

        var profile = new UserProfile
        {
            Username = username,
        };
        _db.UserProfiles.Add(profile);

        await _db.SaveChangesAsync();
        ViewBag.Message = "User successfully registered.";
        return View();
    }


    public IActionResult ForgotPassword()
    {
        if (HttpContext.Session.GetString("username") != null)
            return RedirectToAction("Welcome");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string username, string newPassword)
    {
        if (HttpContext.Session.GetString("username") != null)
            return RedirectToAction("Welcome");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _db.SaveChangesAsync();
            return RedirectToAction("Login");
        }
        ViewBag.Error = "User not found";
        return View();
    }

    public IActionResult Welcome()
    {
        if (HttpContext.Session.GetString("username") == null)
            return RedirectToAction("Login");
        return View();
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
    public async Task<IActionResult> UserList()
{
    var role = HttpContext.Session.GetString("role");
    if (role != "Admin")
        return RedirectToAction("Login"); // lub AccessDenied

  var usersWithProfiles = await _db.Users
    .GroupJoin(_db.UserProfiles,
        u => u.Username,
        p => p.Username,
        (u, profiles) => new { u, profiles })
    .SelectMany(
        up => up.profiles.DefaultIfEmpty(),
        (up, p) => new { up.u, Profile = p })
    .Select(x => new UserListViewModel
    {
        Username = x.u.Username,
        Role = x.u.Role,
        ProfileImageUrl = x.Profile == null || string.IsNullOrEmpty(x.Profile.ImagePath)
            ? "/images/default-profile.png"
            : "/uploads/" + x.Profile.ImagePath
    })
    .ToListAsync();

return View(usersWithProfiles);

}

}

