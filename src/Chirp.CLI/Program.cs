using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using CsvHelper;
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

    public static void Main(string[] args)
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

            UserInterface.PrintCheeps(CheepDatabase.Instance.Read(limit));
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

    private static void AddCheep(string message, HttpClient client)
    {
        //curl -X POST http://localhost:5016/cheep \
        // -H "Content-Type: application/json" \
        // -d '{"author": "havkost", "message": "Hello, world!", "timestamp": 1690891760}'
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var author = Environment.UserName;

        Cheep cheep = new(author, message, timestamp);
        var url = "http://localhost:5016/cheep";
        var jsonCheep = JsonSerializer.Serialize(cheep);
        var header = new MediaTypeHeaderValue("application/json");
        var content = new StringContent(jsonCheep, header);
        var respone = client.PostAsync(url, content);
        if (respone.Result.StatusCode == HttpStatusCode.OK)
        {
            Console.WriteLine("Cheeped message");
        }
    }
}