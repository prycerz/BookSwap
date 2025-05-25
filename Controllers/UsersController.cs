using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSwap.Models;
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
        var userProfile = await _context.UserProfiles
                            .FirstOrDefaultAsync(u => u.Id == id);
        if (userProfile == null)
        {
            return NotFound();
        }
        return View(userProfile);
    }
}
