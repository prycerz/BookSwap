using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = "";

    [Required]
    public string PasswordHash { get; set; } = "";

    public string Role { get; set; } = "User";

    public string Token { get; set; } = Guid.NewGuid().ToString();
}
