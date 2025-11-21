using LibraryApis.Models;

namespace LibraryApis.Data;

public static class Seeder
{
    public static void Seed(LibraryContext db)
    {
        // If there are already books, assume data is seeded
        if (db.Books.Any()) return;

        // 1️⃣ Seed Books
        var books = new[]
        {
            new Book { Title = "Moby Dick", Pages = 635 },
            new Book { Title = "Pride and Prejudice", Pages = 432 },
            new Book { Title = "1984", Pages = 328 },
            new Book { Title = "The Hobbit", Pages = 304 },
            new Book { Title = "Clean Code", Pages = 464 }
        };
        db.Books.AddRange(books);
        db.SaveChanges(); // Save to ensure Ids are generated

        // 2️⃣ Seed Users
        var users = new[]
        {
            new User { Name = "Alice" },
            new User { Name = "Bob" },
            new User { Name = "Carla" }
        };
        db.Users.AddRange(users);
        db.SaveChanges(); // Save to ensure Ids are generated

        // 3️⃣ Seed BorrowRecords
        var now = DateTime.UtcNow;

        var borrowRecords = new[]
        {
            new BorrowRecord
            {
                BookId = books[0].Id,
                UserId = users[0].Id,
                BorrowedAt = now.AddDays(-30),
                ReturnedAt = now.AddDays(-20)
            },
            new BorrowRecord
            {
                BookId = books[0].Id,
                UserId = users[1].Id,
                BorrowedAt = now.AddDays(-25),
                ReturnedAt = now.AddDays(-5)
            },
            new BorrowRecord
            {
                BookId = books[1].Id,
                UserId = users[0].Id,
                BorrowedAt = now.AddDays(-10),
                ReturnedAt = now.AddDays(-3)
            },
            new BorrowRecord
            {
                BookId = books[2].Id,
                UserId = users[2].Id,
                BorrowedAt = now.AddDays(-15),
                ReturnedAt = now.AddDays(-1)
            },
            new BorrowRecord
            {
                BookId = books[3].Id,
                UserId = users[1].Id,
                BorrowedAt = now.AddDays(-7),
                ReturnedAt = null // Not returned yet
            }
        };

        db.BorrowRecords.AddRange(borrowRecords);
        db.SaveChanges();
    }
}
