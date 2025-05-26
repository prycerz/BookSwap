using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSwap.Models;
using System.Threading.Tasks;
using System.Linq;

public class UsersController : Controller
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Users/Profile/5
    public async Task<IActionResult> Profile(int id)
    {
        // Pobierz profil użytkownika po id
        var userProfile = await _context.UserProfiles
                            .FirstOrDefaultAsync(u => u.Id == id);

        if (userProfile == null)
        {
            return NotFound();
        }

        // Pobierz książki przypisane do użytkownika (UserId == userProfile.Id)
        var books = await _context.Books
                        .Where(b => b.UserId == userProfile.Id)
                        .ToListAsync();

        // Przygotuj model widoku
        var model = new UserProfileWithBooksViewModel
        {
            Profile = userProfile,
            Books = books
        };

        return View(model);
    }
}

