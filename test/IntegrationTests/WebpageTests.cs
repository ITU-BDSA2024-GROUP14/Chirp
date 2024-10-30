using Chirp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests;

/// <summary>
/// Tests for the Webpage.
/// </summary>
public class WebpageTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;

    public WebpageTests(CustomWebApplicationFactory<Program> fixture)
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
        var response = await _client.GetAsync("/");
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
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", responseContent);
        Assert.Contains($"{author}'s Timeline", responseContent);
    }

    [Theory]
    [InlineData("Jacqualine Gilcoine", "Starbuck now is what we hear the worst.")]
    [InlineData("Quintin Sitts", "On reaching the end of either, there came a sound so deep an influence over her?")]
    public async void PrivateTimelineHasContent(string author, string expectedContent)
    {
        using var scope = _fixture.Services.CreateScope();
        {
            //Arrange
            var context = scope.ServiceProvider.GetRequiredService<ChirpDBContext>();
            TestData.SeedDatabase(context);
            
            author = author.Replace(" ", "_");
            var response = await _client.GetAsync($"/{author}");
            response.EnsureSuccessStatusCode();

            //Act
            var responseContent = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains(expectedContent, responseContent);
        }
    }
}