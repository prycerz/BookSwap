using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSwap.Models
{
    public class BookWithProfileViewModel
    {
        public Book Book { get; set; }
        public UserProfile? Profile { get; set; }
    }
}