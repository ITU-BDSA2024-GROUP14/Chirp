using System.Globalization;

using CsvHelper;

using DocoptNet;

using SimpleDB;

namespace Chirp.CLI;

internal class Program
{
    public record Cheep(string Author, string Message, long Timestamp);

    public static string DATABASE_PATH = "./chirp_cli_db.csv";

    //Using @ to create verbatim string, which means that no escapes are needed
    //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/verbatim
    private const string Usage = @"Chirp.

Usage:
  chirp read [<limit>]
  chirp cheep <message>
  chirp (-h | --help)

Options:
  -h --help  Show this screen.
";

    public static void Main(string[] args)
    {
        IDatabaseRepository<Cheep> db = new CSVDatabase<Cheep>(DATABASE_PATH);
        IDictionary<string, ValueObject>? arguments = new Docopt().Apply(Usage, args, exit: true);
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

            UserInterface.PrintCheeps(db.Read(limit));
        }
        else if (arguments["cheep"].IsTrue)
        {
            AddCheep(arguments["<message>"].ToString(), db);
        }
    }

    private static void AddCheep(string message, IDatabaseRepository<Cheep> db)
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string author = Environment.UserName;

        Cheep cheep = new(author, message, timestamp);
        db.Store(cheep);
    }
}