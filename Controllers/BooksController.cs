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

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        book.UserId = user.Id;

        _db.Books.Add(book);
        await _db.SaveChangesAsync();
        return RedirectToAction("MyBooks");
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
