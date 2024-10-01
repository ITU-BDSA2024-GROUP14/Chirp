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

    //Test based on https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_05/Slides.html
    [Theory]
    [InlineData("Chirp!")]
    [InlineData("Public Timeline")]
    [InlineData("Chirp &mdash; An ASP.NET Application")]
    public async Task WebpageContainsContent(string expectedContent)
    {
        var response = await _client.GetAsync("");
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains(expectedContent, responseContent);
    }

    //Test copied from https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_05/Slides.html
    [Theory]
    [InlineData("Esser")]
    [InlineData("Roger Histand")]
    [InlineData("Baron Aslan Glorfindus von Fritz")]
    public async void CanSeePrivateTimeline(string author)
    {
        author = author.Replace(" ", "_");
        var response = await _client.GetAsync($"http://localhost/{author}");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", responseContent);
        Assert.Contains($"{author}'s Timeline", responseContent);
    }
}