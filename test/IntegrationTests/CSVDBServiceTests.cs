using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Chirp.CSVDBService;

namespace IntegrationTests;

[Collection("Sequential")] // prevents the tests from running in parallel, preventing port already in use
public class CSVDBServiceTests
{
    private const int Port = 5016;

    [Theory]
    [InlineData("jonv", "hello world")]
    public async Task StoreValidCheep_ShouldReturnOk(string author, string message)
    {
        var response = await CreateProcessAndPostCheep(author, message, 1690891710);

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
        var response = await CreateProcessAndPostCheep(author, message, 1690891710);

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ReadCheep_ShouldReturn()
    {
        using var process = CreateProcess();
        using var httpClient = new HttpClient();

        var response = await httpClient.GetFromJsonAsync<List<Cheep>>($"http://localhost:{Port}/cheeps");

        process.Kill();

        Assert.NotNull(response);
    }

    private Process CreateProcess()
    {
        using var process = new Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CSVDBService";
        process.StartInfo.Arguments = "run";

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;

        process.Start();

        // var reader = process.StandardOutput;
        // var output = reader.ReadToEnd();
        // var regex = new Regex("(?<=localhost:)[0-9]{4}");
        // var port = regex.Match(output).Value;
        // Assert.NotEmpty(port);

        return process;
    }

    private async Task<HttpResponseMessage> CreateProcessAndPostCheep(string author, string message, long timestamp)
    {
        using var process = CreateProcess();

        // call invalid cheep
        using var httpClient = new HttpClient();
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

        var response = await httpClient.PostAsync($"http://localhost:{Port}/cheep", content);

        process.Kill();

        return response;
    }
}