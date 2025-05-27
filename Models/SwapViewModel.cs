using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BookSwap.Models;
public class SwapViewModel
{
    public Book SelectedBook { get; set; }
    public List<Book> UserBooks { get; set; }
}
