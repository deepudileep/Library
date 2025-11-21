namespace LibraryApis.Models;

public class BorrowRecord
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public Book Book { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
}
