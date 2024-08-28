using System.Globalization;
using System.Text.RegularExpressions;

class Program
{

    //Regex and date time information found here:
    //https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.-ctor?view=net-7.0#system-text-regularexpressions-regex-ctor(system-string)
    //https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.utcnow?view=net-7.0
    public static void PrintCheeps()
    {
        var lines = File.ReadAllLines("/home/ostarup/ITU/BDSA/Chirp/Chirp.CLI/chirp_cli_db.csv");
        foreach (var line in lines.Skip(1))
        {
            var regex = new Regex(@"(?:[""],)|(?:,[""])");
            var items = regex.Split(line);
            var author = items[0];
            var message = items[1].Trim('"');
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(items[2]));
            var cheep = author + " @ " + timestamp.DateTime.ToString("dd/MM/yy hh:mm:ss", new CultureInfo("en-DE")) + ": " + message + "\n";
            Console.Write(cheep);
        }
    }

    public static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Error");
            return;
        }

        if (args[0] == "read")
        {
            PrintCheeps();
        }
    }
}
