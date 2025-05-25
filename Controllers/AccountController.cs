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
            return RedirectToAction("Index", "Home");
        }
        ViewBag.Error = "Invalid credentials";
        return View();
    }

    public IActionResult Register()
    {
        if (HttpContext.Session.GetString("username") != null)
            return RedirectToAction("Welcome");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string username, string password)
    {
        if (HttpContext.Session.GetString("username") != null)
            return RedirectToAction("Welcome");

        if (await _db.Users.AnyAsync(u => u.Username == username))
        {
            ViewBag.Error = "Username already taken";
            return View();
        }

        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
        };

        _db.Users.Add(user);

        var profile = new UserProfile
        {
            Username = username,
        };
        _db.UserProfiles.Add(profile);


        await _db.SaveChangesAsync();
        return RedirectToAction("Login");
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
}

