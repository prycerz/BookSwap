using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSwap.Models;
using System.Security.Claims;

public class ConversationsController : Controller
{
    private readonly AppDbContext _context;

    public ConversationsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var currentUserId = HttpContext.Session.GetInt32("userId");
        if (currentUserId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Pobierz wszystkie wiadomości, gdzie user jest nadawcą lub odbiorcą
        var messages = await _context.Messages
            .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .ToListAsync();

        // Pobierz wszystkie UserProfile (można ograniczyć do tych użytkowników, którzy są w wiadomościach)
        var usernames = messages
            .SelectMany(m => new[] { m.Sender.Username, m.Receiver.Username })
            .Distinct()
            .ToList();

        var userProfiles = await _context.UserProfiles
            .Where(up => usernames.Contains(up.Username))
            .ToDictionaryAsync(up => up.Username);

        // Grupujemy konwersacje - druga osoba w rozmowie to Sender lub Receiver
        var conversations = messages
            .GroupBy(m => m.SenderId == currentUserId ? m.Receiver : m.Sender)
            .Select(g =>
            {
                var user = g.Key;
                var profile = userProfiles.ContainsKey(user.Username) ? userProfiles[user.Username] : null;

                return new ConversationListItemViewModel
                {
                    OtherUserId = user.Id,
                    OtherUsername = user.Username,
                    OtherUserProfileImage = profile?.ImagePath,  // pobieramy zdjęcie z profilu
                    LastMessage = g.OrderByDescending(m => m.DateSent).First().Content,
                    LastMessageDate = g.OrderByDescending(m => m.DateSent).First().DateSent
                };
            })
            .OrderByDescending(c => c.LastMessageDate)
            .ToList();

        return View(conversations);
    } 

}
