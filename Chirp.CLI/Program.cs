using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;

class Program
{
    public record Cheep(string Author, string Message, long Timestamp);
    private static string DATABASE_PATH = "./chirp_cli_db.csv";

    private static CultureInfo CULTURE_INFO = new CultureInfo("en-DE");
    //Regex and date time information found here:
    //https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.-ctor?view=net-7.0#system-text-regularexpressions-regex-ctor(system-string)
    //https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.utcnow?view=net-7.0
    public static void PrintCheeps()
    {
        var lines = File.ReadAllLines(DATABASE_PATH);
        foreach (var line in lines.Skip(1))
        {
            var regex = new Regex(@"(?<!"".*),|,(?!.*"")");
            var items = regex.Split(line);
            var author = items[0];
            var message = items[1].Trim('"');
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(items[2]));
            //The following lines takes the date time and converts it to local time, and then prints it in the desired format
            var time = timestamp.DateTime.ToLocalTime();
            var cheep = author + " @ " + time.ToString("dd/MM/yy HH:mm:ss", CULTURE_INFO)+ ": " + message + "\n";
            Console.Write(cheep);
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
        }
    }

    public static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Error");
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
                    Console.WriteLine("Needs more arguments");
                    return;
                }
                CreateCheep(args[1]);
                break;
        }
    }
}
