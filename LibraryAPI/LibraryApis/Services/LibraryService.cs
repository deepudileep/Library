using Grpc.Core;
using LibraryApis.Data;
using Library.Grpc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApis.Serivices;
public class LibraryServiceImpl : LibraryService.LibraryServiceBase
{
    private readonly LibraryContext _db;
    public LibraryServiceImpl(LibraryContext db) { _db = db; }

    public override async Task<TopBooksResponse> GetMostBorrowedBooks(TopBooksRequest req, ServerCallContext context)
    {
 
        // Safe parsing for 'From'
        DateTime from;
        if (!DateTime.TryParse(req.Range?.From, out from))
        {
            from = DateTime.MinValue; // default if null, empty, or invalid
        }

        // Safe parsing for 'To'
        DateTime to;
        if (!DateTime.TryParse(req.Range?.To, out to))
        {
            to = DateTime.MaxValue; // default if null, empty, or invalid
        }
        var limit = req.Limit > 0 ? req.Limit : 10;

        var q = await _db.BorrowRecords
            .Where(b => b.BorrowedAt >= from && b.BorrowedAt <= to)
            .GroupBy(b => b.BookId)
            .Select(g => new { BookId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(limit)
            .ToListAsync();

        var resp = new TopBooksResponse();
        foreach (var item in q)
        {
            var book = await _db.Books.FindAsync(item.BookId);
            resp.Items.Add(new BookCount
            {
                Book = new Book { Id = book.Id, Title = book.Title, Pages = book.Pages },
                BorrowCount = item.Count
            });
        }
        return resp;
    }

    public override async Task<TopUsersResponse> GetTopBorrowers(TopUsersRequest req, ServerCallContext context)
    {
        var from = req.Range?.From != null ? DateTime.Parse(req.Range.From) : DateTime.MinValue;
        var to = req.Range?.To != null ? DateTime.Parse(req.Range.To) : DateTime.MaxValue;
        var limit = req.Limit > 0 ? req.Limit : 10;

        var q = await _db.BorrowRecords
            .Where(b => b.BorrowedAt >= from && b.BorrowedAt <= to)
            .GroupBy(b => b.UserId)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(limit)
            .ToListAsync();

        var resp = new TopUsersResponse();
        foreach (var item in q) resp.Items.Add(new UserCount { UserId = item.UserId, BorrowCount = item.Count });
        return resp;
    }

    public override async Task<ReadingPaceResponse> EstimateReadingPace(ReadingPaceRequest req, ServerCallContext context)
    {
        var rec = await _db.BorrowRecords
            .Include(r => r.Book)
            .Where(r => r.BookId == req.BookId && r.UserId == req.UserId && r.ReturnedAt != null)
            .OrderByDescending(r => r.ReturnedAt)
            .FirstOrDefaultAsync();

        if (rec == null || rec.ReturnedAt == null) return new ReadingPaceResponse { PagesPerDay = 0 };

        var days = (rec.ReturnedAt.Value - rec.BorrowedAt).TotalDays;
        if (days <= 0) days = 1.0; // avoid divide by 0; assume at least 1 day
        var pagesPerDay = rec.Book.Pages / days;
        return new ReadingPaceResponse { PagesPerDay = pagesPerDay };
    }

    public override async Task<AlsoBorrowedResponse> GetAlsoBorrowed(AlsoBorrowedRequest req, ServerCallContext context)
    {
        // Find users who borrowed the book
        var userIds = await _db.BorrowRecords
            .Where(r => r.BookId == req.BookId)
            .Select(r => r.UserId)
            .Distinct()
            .ToListAsync();

        // All other books these users borrowed (excluding the original)
        var coBorrowed = await _db.BorrowRecords
            .Where(r => userIds.Contains(r.UserId) && r.BookId != req.BookId)
            .GroupBy(r => r.BookId)
            .Select(g => new { BookId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(req.Limit > 0 ? req.Limit : 10)
            .ToListAsync();

        var resp = new AlsoBorrowedResponse();
        foreach (var item in coBorrowed)
        {
            var b = await _db.Books.FindAsync(item.BookId);
            resp.Items.Add(new BookCount
            {
                Book = new Book { Id = b.Id, Title = b.Title, Pages = b.Pages },
                BorrowCount = item.Count
            });
        }
        return resp;
    }
}
