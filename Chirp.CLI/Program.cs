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
        var arguments = new Docopt().Apply(Usage, args);
        Console.WriteLine(arguments);
        if (args.Length < 1)
        {
            UserInterface.PrintError("Must have at least one argument");
            return;
        }

        switch (args[0])
        {
            case "read":
                PrintCheeps();
                break;
            case "cheep":
                if (args.Length < 2)
                {
                    UserInterface.PrintError("Needs more arguments");
                    return;
                }
                CreateCheep(args[1]);
                break;
        }
    }
}
