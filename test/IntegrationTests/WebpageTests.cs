using Chirp.Razor;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

public class WebpageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;

    public WebpageTests(WebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
        _client = _fixture.CreateClient(
            new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
    }

    [Theory]
    [InlineData("Chirp!")]
    [InlineData("Public Timeline")]
    [InlineData("Chirp — An ASP.NET Application")]
    public async Task WebpageContainsContent(string expectedContent)
    {
        var response = await _client.GetAsync("/");
    
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains(responseContent, expectedContent);
    }
}