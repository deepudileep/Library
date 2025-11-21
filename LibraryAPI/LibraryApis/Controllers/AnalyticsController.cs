
using Library.Grpc;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly LibraryService.LibraryServiceClient _client;
    public AnalyticsController(LibraryService.LibraryServiceClient client) { _client = client; }

    [HttpGet("most-borrowed")]
    public async Task<IActionResult> MostBorrowed([FromQuery] int limit = 10)
    {
        var req = new TopBooksRequest { Limit = limit, Range = new TimeRange { From = "", To = "" } };
        var r = await _client.GetMostBorrowedBooksAsync(req);
        return Ok(r.Items.Select(i => new { i.Book.Id, i.Book.Title, i.BorrowCount }));
    }

    [HttpGet("top-borrowers")]
    public async Task<IActionResult> TopBorrowers([FromQuery] int limit = 10)
    {
        var r = await _client.GetTopBorrowersAsync(new TopUsersRequest { Limit = limit });
        return Ok(r.Items.Select(i => new { i.UserId, i.BorrowCount }));
    }

    [HttpGet("reading-pace")]
    public async Task<IActionResult> ReadingPace([FromQuery] long userId, [FromQuery] long bookId)
    {
        var r = await _client.EstimateReadingPaceAsync(new ReadingPaceRequest { UserId = userId, BookId = bookId });
        return Ok(new { pagesPerDay = r.PagesPerDay });
    }

    [HttpGet("also-borrowed/{bookId}")]
    public async Task<IActionResult> AlsoBorrowed(long bookId, [FromQuery] int limit = 10)
    {
        var r = await _client.GetAlsoBorrowedAsync(new AlsoBorrowedRequest { BookId = bookId, Limit = limit });
        return Ok(r.Items.Select(i => new { i.Book.Id, i.Book.Title, i.BorrowCount }));
    }
}
