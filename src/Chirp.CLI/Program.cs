using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using DocoptNet;
using SimpleDB;

namespace Chirp.CLI;

internal class Program
{
    //Using @ to create verbatim string, which means that no escapes are needed
    //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/verbatim
    private const string Usage = @"Chirp.

Usage:
  chirp read [-d <databasepath>]
  chirp read [<limit>] [-d <databasepath>]
  chirp cheep <message> [-d <databasepath>]
  chirp (-h | --help)

Options:
  -h --help  Show this screen.
";

    public static async Task Main(string[] args)
    {
        var client = new HttpClient();
        var arguments = new Docopt().Apply(Usage, args, exit: true);
        if (arguments == null)
        {
            throw new NullReferenceException("CLI argument parsing failed\narguments is null");
        }

        if (arguments["read"].IsTrue)
        {
            int? limit = null;
            if (arguments["<limit>"].IsInt)
            {
                limit = arguments["<limit>"].AsInt;
            }

            if (arguments["-d"].IsTrue && !arguments["<databasepath>"].IsNullOrEmpty)
            {
                var databasePath = arguments["<databasepath>"].ToString();
                CheepDatabase.Instance.ChangeCsvPath(databasePath);
            }

            await readCheeps(client, limit);
        }
        else if (arguments["cheep"].IsTrue)
        {
            if (arguments["-d"].IsTrue && !arguments["<databasepath>"].IsNullOrEmpty)
            {
                var databasePath = arguments["<databasepath>"].ToString();
                CheepDatabase.Instance.ChangeCsvPath(databasePath);
            }

            AddCheep(arguments["<message>"].ToString(), client);
        }
    }

    private static string website =
        "https://bdsagroup14chirpremotedb2024-h7a5c2ahfqhgcag3.northeurope-01.azurewebsites.net/";

    private static async Task readCheeps(HttpClient client, int? limit = null)
    {
        var uri = website + "cheeps";
        var response = await client.GetAsync(uri);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new HttpRequestException("Response unsuccessful, got: " + response.StatusCode);
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var cheeps = JsonSerializer.Deserialize<List<Cheep>>(jsonResponse, options);
        if (cheeps == null)
        {
            throw new NullReferenceException("Cheeps are null");
        }

        UserInterface.PrintCheeps(cheeps);
    }

    private static void AddCheep(string message, HttpClient client)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var author = Environment.UserName;

        Cheep cheep = new(author, message, timestamp);
        var uri = website + "cheep";
        var jsonCheep = JsonSerializer.Serialize(cheep);
        var header = new MediaTypeHeaderValue("application/json");
        var content = new StringContent(jsonCheep, header);
        var respone = client.PostAsync(uri, content);
        if (respone.Result.StatusCode == HttpStatusCode.OK)
        {
            Console.WriteLine("Cheeped message");
        }
        else
        {
            Console.WriteLine("Failed got respone: " + respone.Result);
        }
    }
}