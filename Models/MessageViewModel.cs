using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BookSwap.Models
{

    public class MessageViewModel
    {
        public int ReceiverId { get; set; }
        public string ReceiverUsername { get; set; }
        public string? ReceiverImagePath { get; set; }
        public int CurrentUserId { get; set; }

        public List<Message> Messages { get; set; } = new();

        [Required]
        [Display(Name = "Napisz wiadomość")]
        public string NewMessage { get; set; }
    }
}