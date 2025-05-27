using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BookSwap.Models;

public class SwapConfirmViewModel
{
    public Book TargetBook { get; set; }
    public Book OfferedBook { get; set; }
    public int? TargetBookOwnerId { get; set; }
    public int? OfferedBookOwnerId { get; set; }
}
