using LibraryApis.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class SeederTests
{
    [Fact]
    public void Seed_ShouldPopulateDatabase_WhenEmpty()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using var context = new LibraryContext(options);
        Seeder.Seed(context);

        Assert.NotEmpty(context.Books);
        Assert.NotEmpty(context.Users);
    }
}
