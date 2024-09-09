using System.Globalization;
using System.Text.RegularExpressions;

namespace Chirp.CLI;

class UserInterface
{
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