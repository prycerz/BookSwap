using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSwap.Models
{
    public class SwapRequest

    {
        public int Id { get; set; }

        public int OfferedBookId { get; set; }
        public Book OfferedBook { get; set; }

        public int TargetBookId { get; set; }
        public Book TargetBook { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected
    } 

}
