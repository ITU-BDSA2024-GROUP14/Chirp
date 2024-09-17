using System.Globalization;
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
  chirp read [<limit>] [<databasepath>]
  chirp cheep <message> [<databasepath>]
  chirp (-h | --help)

Options:
  -h --help  Show this screen.
";

    public static void Main(string[] args)
    {
        var arguments = new Docopt().Apply(Usage, args, exit: true);
        if (arguments == null)
        {
            throw new NullReferenceException("CLI argument parsing failed\narguments is null");
        }

        if (arguments["read"].IsTrue)
        {
            string? databasePath = null;
            int? limit = null;
            if (arguments["<limit>"].IsInt)
            {
                limit = arguments["<limit>"].AsInt;
            }

            if (arguments["<limit>"].IsString)
            {
                databasePath = arguments["<limit>"].ToString();
                CheepDatabase.Instance.ChangeCsvPath(databasePath);
            }

            UserInterface.PrintCheeps(CheepDatabase.Instance.Read(limit));
        }
        else if (arguments["cheep"].IsTrue)
        {
            AddCheep(arguments["<message>"].ToString(), CheepDatabase.Instance);
        }
    }

    private static void AddCheep(string message, IDatabaseRepository<Cheep> db)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var author = Environment.UserName;

        Cheep cheep = new(author, message, timestamp);
        db.Store(cheep);
    }
}