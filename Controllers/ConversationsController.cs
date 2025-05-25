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
        .OrderByDescending(m => m.DateSent)
        .ToListAsync();

    // Znajdź ID innych użytkowników z konwersacji
    var otherUserIds = messages
        .Select(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
        .Distinct()
        .ToList();

    // Pobierz profile UserProfiles po Id (tych innych użytkowników)
    var userProfiles = await _context.UserProfiles
        .Where(up => otherUserIds.Contains(up.Id))
        .ToDictionaryAsync(up => up.Id);

    // Grupujemy konwersacje po drugim użytkowniku (po Id)
    var conversations = otherUserIds.Select(otherUserId =>
    {
        // ostatnia wiadomość między currentUser a otherUser
        var lastMessage = messages
            .Where(m => (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                        (m.SenderId == otherUserId && m.ReceiverId == currentUserId))
            .OrderByDescending(m => m.DateSent)
            .FirstOrDefault();

        userProfiles.TryGetValue(otherUserId, out var profile);

        return new ConversationListItemViewModel
        {
            OtherUserId = otherUserId,
            OtherUsername = profile?.Username ?? "Nieznany",
            OtherUserProfileImage = profile?.ImagePath,
            LastMessage = lastMessage?.Content ?? "",
            LastMessageDate = lastMessage?.DateSent ?? DateTime.MinValue
        };
    })
    .OrderByDescending(c => c.LastMessageDate)
    .ToList();

    return View(conversations);
}


}
