using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSwap.Models;

public class ProfileController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ProfileController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public IActionResult Index()
    {
        var username = HttpContext.Session.GetString("username");
        if (username == null) return RedirectToAction("Login", "Account");

        var profile = _db.UserProfiles.FirstOrDefault(u => u.Username == username);
        if (profile == null)
        {
            profile = new UserProfile { Username = username };
            _db.UserProfiles.Add(profile);
            _db.SaveChanges();
        }

        var books = _db.Books
            .Where(b => b.UserId == profile.Id)
            .ToList();

        var model = new UserProfileWithBooksViewModel
        {
            Profile = profile,
            Books = books
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile avatar)
    {
        var username = HttpContext.Session.GetString("username");
        if (username == null || avatar == null) return RedirectToAction("Index");

        var profile = _db.UserProfiles.FirstOrDefault(u => u.Username == username);
        if (profile == null)
        {
            profile = new UserProfile { Username = username };
            _db.UserProfiles.Add(profile);
            await _db.SaveChangesAsync();
        }

        var ext = Path.GetExtension(avatar.FileName).ToLower();
        if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
        {
            ModelState.AddModelError("", "Only JPG or PNG files allowed.");
            return RedirectToAction("Index");
        }

        var fileName = $"{Guid.NewGuid()}{ext}";
        var path = Path.Combine(_env.WebRootPath, "uploads", fileName);

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await avatar.CopyToAsync(stream);
        }

        if (!string.IsNullOrEmpty(profile.ImagePath))
        {
            var oldPath = Path.Combine(_env.WebRootPath, "uploads", profile.ImagePath);
            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
        }

        profile.ImagePath = fileName;
        _db.UserProfiles.Update(profile);
        await _db.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete()
    {
        var username = HttpContext.Session.GetString("username");
        if (username == null) return RedirectToAction("Login", "Account");

        var profile = await _db.UserProfiles.FirstOrDefaultAsync(u => u.Username == username);
        if (profile == null) return RedirectToAction("Index");

        if (!string.IsNullOrEmpty(profile.ImagePath))
        {
            var filePath = Path.Combine(_env.WebRootPath, "uploads", profile.ImagePath);
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

            profile.ImagePath = null;
            _db.UserProfiles.Update(profile);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}

