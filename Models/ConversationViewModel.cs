using System.ComponentModel.DataAnnotations;
namespace BookSwap.Models;

public class ConversationListItemViewModel
{
    public int OtherUserId { get; set; }
    public string OtherUsername { get; set; }
    public string? OtherUserProfileImage { get; set; }
    public string LastMessage { get; set; }
    public DateTime LastMessageDate { get; set; }
}
