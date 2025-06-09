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
    return View(); // Każdy ma dostęp
}



 [HttpPost]
public async Task<IActionResult> Register(string username, string password)
{
    if (await _db.Users.AnyAsync(u => u.Username == username))
    {
        ViewBag.Error = "Username already taken";
        return View();
    }

    var user = new User
    {
        Username = username,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
        Role = "User" // lub nadawane dynamicznie
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
[HttpPost]
public async Task<IActionResult> DeleteUser(string username)
{
    var role = HttpContext.Session.GetString("role");
    if (role != "Admin")
        return RedirectToAction("Login");

    // Znajdź użytkownika
    var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
    if (user == null)
        return NotFound();

    // Usuń książki powiązane z user.Id
    var booksToDelete = _db.Books.Where(b => b.UserId == user.Id);
    _db.Books.RemoveRange(booksToDelete);

    // Usuń wiadomości gdzie SenderId lub ReceiverId == user.Id
    var messagesToDelete = _db.Messages.Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id);
    _db.Messages.RemoveRange(messagesToDelete);

    // Usuń SwapRequests gdzie OfferedBookOwnerId lub TargetBookOwnerId == user.Id
    var swapRequestsToDelete = _db.SwapRequests.Where(s => s.OfferedBookOwnerId == user.Id || s.TargetBookOwnerId == user.Id);
    _db.SwapRequests.RemoveRange(swapRequestsToDelete);

    // Usuń profil użytkownika (UserProfile) gdzie Id == user.Id
    var profileToDelete = await _db.UserProfiles.FirstOrDefaultAsync(p => p.Id == user.Id);
    if (profileToDelete != null)
        _db.UserProfiles.Remove(profileToDelete);

    // Usuń samego użytkownika
    _db.Users.Remove(user);

    await _db.SaveChangesAsync();

    return RedirectToAction("UserList");
}


}

