using System.Globalization;
using CsvHelper;

namespace Chirp.CLI;

using DocoptNet;

class Program
{
    public record Cheep(string Author, string Message, long Timestamp);
    public static string DATABASE_PATH = "./chirp_cli_db.csv";
    public static CultureInfo CULTURE_INFO = new CultureInfo("en-DE");

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

    //Regex and date time information found here:
    //https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.-ctor?view=net-7.0#system-text-regularexpressions-regex-ctor(system-string)
    //https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.utcnow?view=net-7.0

    public static void Main(string[] args)
    {
        var arguments = new Docopt().Apply(Usage, args, exit: true);
        if (arguments == null) throw new NullReferenceException("CLI argument parsing failed\narguments is null");

        if (arguments["read"].IsTrue)
        {
            PrintCheeps();
        } else if (arguments["cheep"].IsTrue)
        {
            CreateCheep(arguments["<message>"].ToString());
        }
    }
    
    static void PrintCheeps()
    {
        using (var reader = new StreamReader(DATABASE_PATH))
        using (var csv = new CsvReader(reader, CULTURE_INFO))
        {
            var records = csv.GetRecords<Cheep>();
            foreach (var cheep in records)
            {
                UserInterface.PrintCheep(cheep);
            }
        }
    }

    private static void CreateCheep(string message)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var author = Environment.UserName;

        var cheep = new Cheep(author, message, timestamp);

        using (var stream = File.Open(DATABASE_PATH, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, CULTURE_INFO))
        {
            csv.WriteRecord(cheep);
            csv.NextRecord();
        }
    }
}
