using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSwap.Models
{
    public class SwapRequest
    {
        public int Id { get; set; }

        [Required]
        public int OfferedBookId { get; set; }

        [Required]
        public int TargetBookId { get; set; }

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        public bool IsAccepted { get; set; } = false;

        [ForeignKey("OfferedBookId")]
        public Book OfferedBook { get; set; }

        [ForeignKey("TargetBookId")]
        public Book TargetBook { get; set; }
    }
}
