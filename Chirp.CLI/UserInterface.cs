using System.Globalization;
using System.Text.RegularExpressions;

namespace Chirp.CLI;

public class UserInterface
{
    public static void PrintCheep(String cheep)
    {
        var regex = new Regex(@"(?<!"".*),|,(?!.*"")");
        var items = regex.Split(cheep);
        var author = items[0];
        var message = items[1].Trim('"');
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(items[2]));
        //The following lines takes the date time and converts it to local time, and then prints it in the desired format
        var time = timestamp.DateTime.ToLocalTime();
        var cheepString = author + " @ " + time.ToString("dd/MM/yy HH:mm:ss", new CultureInfo("en-DE")) + ": " + message + "\n";
        Console.Write(cheepString);
    }

    public static void PrintError(String error)
    {
        Console.WriteLine("Error: " + error);
    }
}