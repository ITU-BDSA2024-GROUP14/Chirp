using System.Globalization;

using SimpleDB;

namespace Chirp.CLI;

class UserInterface
{
    public static void PrintCheeps(IEnumerable<Program.Cheep> records)
    {
        foreach (var cheep in records)
        {
            PrintCheep(cheep);
        }
    }

    public static void PrintCheep(Program.Cheep cheep)
    {
        var author = cheep.Author;
        var message = cheep.Message;
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).UtcDateTime;
        var outputString = author + " @ " + timestamp.ToString("dd/MM/yy HH:mm:ss", new CultureInfo("en-DE")) + ": " + message + "\n";
        Console.Write(outputString);
    }

    public static void PrintError(String error)
    {
        Console.WriteLine("Error: " + error);
    }
}