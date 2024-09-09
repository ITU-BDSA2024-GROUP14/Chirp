﻿using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;

using Chirp.CLI;

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
        using (var reader = new StreamReader(DATABASE_PATH))
        using (var csv = new CsvReader(reader, CULTURE_INFO))
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

    public static void Main(string[] args)
    {
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
