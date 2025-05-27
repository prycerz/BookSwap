using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSwap.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookSwap.Controllers

{
    public class SwapRequestController : Controller
    {
        private readonly AppDbContext _db;

        public SwapRequestController(AppDbContext db)
        {
            _db = db;
        }

        // GET: SwapRequest/Confirm?targetBookId=...&offeredBookId=...
       public async Task<IActionResult> Confirm(int targetBookId, int offeredBookId)
    {
        var targetBook = await _db.Books.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == targetBookId);
        var offeredBook = await _db.Books.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == offeredBookId);

        if (targetBook == null || offeredBook == null)
            return NotFound();

        var model = new SwapConfirmViewModel
        {
            TargetBook = targetBook,
            OfferedBook = offeredBook,
            TargetBookOwnerId = targetBook.User?.Id,
            OfferedBookOwnerId = offeredBook.User?.Id
        };

        return View(model);
    }
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(int targetBookId, int offeredBookId)
{
    var targetBook = await _db.Books.FindAsync(targetBookId);
    var offeredBook = await _db.Books.FindAsync(offeredBookId);

    if (targetBook == null || offeredBook == null)
    {
        return NotFound();
    }

    var swapRequest = new SwapRequest
    {
        TargetBookId = targetBookId,
        OfferedBookId = offeredBookId,
        CreatedAt = DateTime.UtcNow,
        Status = "Pending",
        TargetBookOwnerId = targetBook.UserId,   // zakładam, że w Book masz UserId
        OfferedBookOwnerId = offeredBook.UserId
    };

    _db.SwapRequests.Add(swapRequest);
    await _db.SaveChangesAsync();

    return RedirectToAction("Index", "Home");
}

    }
    
}