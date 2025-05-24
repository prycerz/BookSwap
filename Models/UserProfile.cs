using System.ComponentModel.DataAnnotations;

namespace BookSwap.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } // z sesji

        public string? ImagePath { get; set; }
    }
}

