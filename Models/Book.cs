using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookSwap.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [StringLength(10, ErrorMessage = "Tytuł nie może przekraczać 10 znaków")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Opis jest wymagany")]
        [StringLength(100, ErrorMessage = "Opis nie może przekraczać 100 znaków")]
        public string Description { get; set; }

        public string ImageUrl { get; set; }
        public int UserId { get; set; }
        public DateTime DateAdded { get; set; }
        public User? User { get; set; }
    }
}
