using System.Globalization;
using System.Text.RegularExpressions;

using Chirp.CLI;

using DocoptNet;

class Program
{
    private static string _path = "./chirp_cli_db.csv";

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
    public static void PrintCheeps()
    {
        var lines = File.ReadAllLines(_path);
        foreach (var line in lines.Skip(1))
        {
            UserInterface.PrintCheep(line);
        }
    }

    private static void CreateCheep(string message)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var author = Environment.UserName;

        var cheep = author + ",\"" + message + "\"," + timestamp;

        using var writer = File.AppendText(_path);

        writer.WriteLine(cheep);

    }

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
}
