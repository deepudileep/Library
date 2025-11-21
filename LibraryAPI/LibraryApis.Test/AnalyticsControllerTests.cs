using LibraryApis;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using Xunit;

public class AnalyticsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public AnalyticsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetMostBorrowed_ReturnsBooks()
    {
        var response = await _client.GetFromJsonAsync<List<object>>("/api/analytics/most-borrowed?limit=5");
        Assert.NotNull(response);
        Assert.True(response.Count <= 5);
    }
}
