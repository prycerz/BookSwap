namespace BookSwap.Models
{
    public class UserProfileWithBooksViewModel
    {
        public UserProfile Profile { get; set; }
        public List<Book> Books { get; set; }
    }
}
