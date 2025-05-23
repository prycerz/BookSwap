using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSwap.Models;

public class BooksController : Controller
{
    private readonly AppDbContext _db;

    public BooksController(AppDbContext db) => _db = db;

    public async Task<IActionResult> MyBooks()
    {
        var username = HttpContext.Session.GetString("username");
        if (username == null) return RedirectToAction("Login", "Account");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        var books = await _db.Books.Where(b => b.UserId == user.Id).ToListAsync();

        return View(books);
    }

    [HttpGet]
    public IActionResult Add()
    {
        if (HttpContext.Session.GetString("username") == null)
            return RedirectToAction("Login", "Account");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Book book)
    {
        var username = HttpContext.Session.GetString("username");
        if (username == null) return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid)
        {
            // Jeśli dane są nieprawidłowe, zwróć widok z błędami, zachowując dane
            return View(book);
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        book.UserId = user.Id;
        book.DateAdded = DateTime.UtcNow;

        _db.Books.Add(book);
        await _db.SaveChangesAsync();
        return RedirectToAction("MyBooks");
    }


    [HttpGet]
    public async  Task<IActionResult> Edit(int id)
    {
        if (HttpContext.Session.GetString("username") == null)
            return RedirectToAction("Login", "Account");
        var book = await _db.Books.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);
        if (book == null) return NotFound();
        return View(book);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Book book)
    {
        var username = HttpContext.Session.GetString("username");
        if (username == null) return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid)
        {
            // Jeśli dane są nieprawidłowe, zwróć widok z błędami, zachowując dane
            return View(book);
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);

        var bookToUpdate = _db.Books.FirstOrDefault(b => b.Id == book.Id);
        if(bookToUpdate == null) return NotFound();

        if (bookToUpdate != null)
        {
            bookToUpdate.Title = book.Title;
            bookToUpdate.Description = book.Description;
            bookToUpdate.ImageUrl = book.ImageUrl;
            // Update any other fields you need
            await _db.SaveChangesAsync();
            return RedirectToAction("MyBooks");
        }
        else
        {
            ModelState.AddModelError("", "Book not found");
            return View(book);
        }

    }
    public async Task<IActionResult> Details(int id)
    {
        var book = await _db.Books.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);
        if (book == null) return NotFound();
        return View(book);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var username = HttpContext.Session.GetString("username");
        if (username == null) return RedirectToAction("Login", "Account");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);
        if (book == null) return NotFound();

        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
        return RedirectToAction("MyBooks");
    }

}
