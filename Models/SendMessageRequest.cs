using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BookSwap.Models;

public class SendMessageRequest
{
    public int ReceiverId { get; set; }
    public string Content { get; set; } = "";
}