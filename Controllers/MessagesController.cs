using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSwap.Models;

namespace BookSwap.Controllers
{
    public class MessagesController : Controller
    {
        private readonly AppDbContext _context;

        public MessagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Messages/Create?userId=2
        public async Task<IActionResult> Create(int userId)
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var receiver = await _context.UserProfiles.FindAsync(userId);
            if (receiver == null)
            {
                return NotFound();
            }

            var model = new MessageViewModel
            {
                ReceiverId = userId,
                ReceiverUsername = receiver.Username,
                ReceiverImagePath = receiver.ImagePath,
                CurrentUserId = currentUser.Id,
                Messages = await _context.Messages
                    .Where(m => (m.SenderId == currentUser.Id && m.ReceiverId == userId)
                             || (m.SenderId == userId && m.ReceiverId == currentUser.Id))
                    .OrderBy(m => m.DateSent)
                    .ToListAsync()
            };

            return View(model);
        }

        // POST: Messages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageViewModel model)
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var message = new Message
                {
                    SenderId = currentUser.Id,
                    ReceiverId = model.ReceiverId,
                    Content = model.NewMessage,
                    DateSent = DateTime.UtcNow
                };
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Create), new { userId = model.ReceiverId });
            }

            var receiver = await _context.UserProfiles.FindAsync(model.ReceiverId);
            model.ReceiverUsername = receiver?.Username ?? "";
            model.ReceiverImagePath = receiver?.ImagePath;
            model.CurrentUserId = currentUser.Id;
            model.Messages = await _context.Messages
                .Where(m => (m.SenderId == currentUser.Id && m.ReceiverId == model.ReceiverId)
                         || (m.SenderId == model.ReceiverId && m.ReceiverId == currentUser.Id))
                .OrderBy(m => m.DateSent)
                .ToListAsync();

            return View(model);
        }
    }
}
