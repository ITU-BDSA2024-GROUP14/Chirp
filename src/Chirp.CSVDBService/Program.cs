using Chirp.CSVDBService;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var database = CheepDatabase.Instance;


app.MapPost("/cheep", Results<BadRequest<string>, Ok> (Cheep cheep) =>
{
    if (string.IsNullOrWhiteSpace(cheep.Author))
    {
        return TypedResults.BadRequest("Author is required");
    }

    if (string.IsNullOrWhiteSpace(cheep.Message))
    {
        return TypedResults.BadRequest("Message is required");
    }

    database.Store(cheep);

    return TypedResults.Ok();
});
app.MapGet("/cheeps", (int? limit = null) => database.Read(limit));

app.Run();

// required per https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program;