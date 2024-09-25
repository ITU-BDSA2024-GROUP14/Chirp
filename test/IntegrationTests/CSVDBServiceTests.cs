using System.Net;
using System.Net.Http.Json;
using System.Text;
using Chirp.CSVDBService;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

[Collection("Sequential")] // prevents the tests from running in parallel, preventing port already in use
public class CSVDBServiceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CSVDBServiceTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("jonv", "hello world")]
    public async Task StoreValidCheep_ShouldReturnOk(string author, string message)
    {
        var response = await PostCheep(author, message, 1690891710);

        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, "")]
    [InlineData("null", null)]
    [InlineData("", "")]
    [InlineData("jonv", null)]
    [InlineData("jonv", "")]
    [InlineData(null, "hello world")]
    [InlineData("", "hello world")]
    public async Task StoreInvalidCheep_ShouldFail(string? author, string? message)
    {
        var response = await PostCheep(author, message, 1690891710);

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ReadCheep_ShouldReturn()
    {
        // Arrange
        var client = _factory.CreateClient();

        var response = await client.GetFromJsonAsync<List<Cheep>>("/cheeps");

        Assert.NotNull(response);
    }

    private async Task<HttpResponseMessage> PostCheep(string author, string message, long timestamp)
    {
        // call invalid cheep
        using var httpClient = _factory.CreateClient();
        var payload = """
                      {
                          "author": "{author}",
                          "message": "{message}",
                          "timestamp": {timestamp}
                      }
                      """;
        payload = payload.Replace("{author}", author);
        payload = payload.Replace("{message}", message);
        payload = payload.Replace("{timestamp}", timestamp.ToString());

        HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("/cheep", content);

        return response;
    }
}