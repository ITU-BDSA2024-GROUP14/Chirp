using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
namespace Chirp.CLI;

public class UserInterface
{
    public static void PrintCheeps()
    {
        using (var reader = new StreamReader(Program.DATABASE_PATH))
        using (var csv = new CsvReader(reader, Program.CULTURE_INFO))
        {
            var records = csv.GetRecords<dynamic>();
            foreach (var cheep in records)
            {
                var author = cheep.Author;
                var message = cheep.Message;
                var timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(cheep.Timestamp)).UtcDateTime;
                var printCheep = author + " @ " + timestamp.ToString("dd/MM/yy HH:mm:ss", new CultureInfo("en-DE")) + ": " + message + "\n";
                Console.Write(printCheep);
            }
        }
    }

    public static void PrintError(String error)
    {
        Console.WriteLine("Error: " + error);
    }
}