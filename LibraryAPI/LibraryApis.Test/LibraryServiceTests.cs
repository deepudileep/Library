using Grpc.Net.Client;
using Library.Grpc;
using Xunit;

public class LibraryServiceTests
{
    [Fact]
    public async Task GetMostBorrowedBooks_ReturnsExpectedData()
    {
        // Use TestServer or in-memory host
        using var channel = GrpcChannel.ForAddress("https://localhost:7145");
        var client = new LibraryService.LibraryServiceClient(channel);

        var response = await client.GetMostBorrowedBooksAsync(new TopBooksRequest { Limit = 3 });

        Assert.NotNull(response);
        Assert.InRange(response.Items.Count, 0, 3);
    }
}
