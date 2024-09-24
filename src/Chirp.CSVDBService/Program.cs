using Chirp.CSVDBService;
using Chirp.CSVDBService.Models;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var database = CheepDatabase.Instance;

if (app.Environment.IsDevelopment())
{
    database.DatabasePath = "../../data/chirp_cli_dev_db.csv";
}

app.MapPost("/cheep", Results<BadRequest<string>, Ok> (CreateCheepRequestModel request) =>
{
    if (string.IsNullOrWhiteSpace(request.Author))
    {
        return TypedResults.BadRequest("Author is required");
    }

    if (string.IsNullOrWhiteSpace(request.Message))
    {
        return TypedResults.BadRequest("Message is required");
    }

    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    database.Store(new Cheep(request.Author, request.Message, timestamp));

    return TypedResults.Ok();
});
app.MapGet("/cheeps", (int? limit = null) => database.Read(limit));

app.Run();

// required per https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program;