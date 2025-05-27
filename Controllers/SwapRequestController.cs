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
            {
                return NotFound();
            }

            var model = new SwapConfirmViewModel
            {
                TargetBook = targetBook,
                OfferedBook = offeredBook
            };

            return View(model);
        }
    }
}