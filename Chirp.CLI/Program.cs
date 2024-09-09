using System.Globalization;
using CsvHelper;
using DocoptNet;
using SimpleDB;

namespace Chirp.CLI;


class Program
{
    public record Cheep(string Author, string Message, long Timestamp);
    public static string DATABASE_PATH = "./chirp_cli_db.csv";

    //Using @ to create verbatim string, which means that no escapes are needed
    //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/verbatim
    private const string Usage = @"Chirp.

Usage:
  chirp read
  chirp cheep <message>
  chirp (-h | --help)

Options:
  -h --help  Show this screen.
";

    public static void Main(string[] args)
    {
        IDatabaseRepository<Cheep> db = new CSVDatabase<Cheep>(DATABASE_PATH);
        var arguments = new Docopt().Apply(Usage, args, exit: true);
        if (arguments == null) throw new NullReferenceException("CLI argument parsing failed\narguments is null");

        if (arguments["read"].IsTrue)
        {
            UserInterface.PrintCheeps(db.Read());
        } else if (arguments["cheep"].IsTrue)
        {
            AddCheep(arguments["<message>"].ToString(), db);
        }
    }
    
    private static void AddCheep(string message, IDatabaseRepository<Cheep> db)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var author = Environment.UserName;
        
        var cheep = new Cheep(author, message, timestamp);
        db.Store(cheep);
    }
}
