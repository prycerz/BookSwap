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
                TargetBookOwnerId = targetBook.UserId,
                OfferedBookOwnerId = offeredBook.UserId
            };

            _db.SwapRequests.Add(swapRequest);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
public IActionResult Decision(int id)
{
    var swap = _db.SwapRequests
        .Include(s => s.OfferedBook).ThenInclude(b => b.User)
        .Include(s => s.TargetBook).ThenInclude(b => b.User)
        .FirstOrDefault(s => s.Id == id);

    if (swap == null)
        return NotFound();

    var currentUsername = HttpContext.Session.GetString("username");
    if (string.IsNullOrEmpty(currentUsername))
        return RedirectToAction("Login", "Account"); // albo inna obsługa braku sesji

    var currentUser = _db.Users.FirstOrDefault(u => u.Username == currentUsername);
    if (currentUser == null)
        return Unauthorized();

    ViewData["CurrentUserId"] = currentUser.Id;
    ViewData["SwapId"] = swap.Id;

    var model = new SwapConfirmViewModel
    {
        OfferedBook = swap.OfferedBook,
        TargetBook = swap.TargetBook
    };

    return View("SwapDecision", model);
}



[HttpPost]
public IActionResult Accept(int id)
{
    var swap = _db.SwapRequests.FirstOrDefault(s => s.Id == id);
    if (swap == null) return NotFound();

    swap.Status = "Accepted";
    _db.SaveChanges();

    return RedirectToAction("Index", "Swaps");
}

[HttpPost]
public IActionResult Decline(int id)
{
    var swap = _db.SwapRequests.FirstOrDefault(s => s.Id == id);
    if (swap == null) return NotFound();

    swap.Status = "Rejected";
    _db.SaveChanges();

    return RedirectToAction("Index", "Swaps");
}





    }
    
}