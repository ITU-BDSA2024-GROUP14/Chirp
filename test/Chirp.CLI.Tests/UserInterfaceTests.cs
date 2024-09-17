using SimpleDB;

namespace Chirp.CLI.Tests;

public class UserInterfaceTests
{
    [Fact]
    public void PrintCheepContainsContent()
    {
        using (StringWriter sw = new())
        {
            Console.SetOut(sw);

            Cheep cheep = new("Jens Petersen", "Min ko har horn", -446752800);
            UserInterface.PrintCheep(cheep);

            string result = sw.ToString();
            Assert.Contains("Jens Petersen", result);
            Assert.Contains("Min ko har horn", result);
            Assert.Contains("05/11/55 06:00:00", result);
        }
    }
}