using LibraryApis.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApis.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<User> Users => Set<User>();
    public DbSet<BorrowRecord> BorrowRecords => Set<BorrowRecord>();
}
