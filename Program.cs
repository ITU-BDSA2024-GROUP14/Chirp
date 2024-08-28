using System.Globalization;
using System.Text.RegularExpressions;

class Program
{
    private static string _path = "/home/ostarup/ITU/BDSA/Chirp/Chirp.CLI/chirp_cli_db.csv";

    //Regex and date time information found here:
    //https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.-ctor?view=net-7.0#system-text-regularexpressions-regex-ctor(system-string)
    //https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.utcnow?view=net-7.0
    public static void PrintCheeps()
    {
        var lines = File.ReadAllLines(_path);
        foreach (var line in lines.Skip(1))
        {
            var regex = new Regex(@"(?<!"".*),|,(?!.*"")");
            var items = regex.Split(line);
            var author = items[0];
            var message = items[1].Trim('"');
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(items[2]));
            //The following lines takes the date time and converts it to local time, and then prints it in the desired format
            var time = timestamp.DateTime.ToLocalTime();
            var cheep = author + " @ " + time.ToString("dd/MM/yy HH:mm:ss", new CultureInfo("en-DE")) + ": " + message + "\n";
            Console.Write(cheep);
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
