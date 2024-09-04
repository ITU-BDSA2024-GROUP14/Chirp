using Chirp.CLI;

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

const string path = "./chirp_cli_db.csv";

static void PrintCheeps()
{
    var lines = File.ReadAllLines(path);
    foreach (var line in lines.Skip(1))
    {
        UserInterface.PrintCheep(line);
    }
}

static void CreateCheep(string message)
{
    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    var author = Environment.UserName;

    var cheep = author + ",\"" + message + "\"," + timestamp;

    using var writer = File.AppendText(path);

    writer.WriteLine(cheep);
}