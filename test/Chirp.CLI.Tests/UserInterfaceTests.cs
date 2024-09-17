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
    
    [Fact]
    public void PrintCheepsContainsContent()
    {
        using (StringWriter sw = new())
        {
            Console.SetOut(sw);

            Cheep[] cheeps = new Cheep[]
            {
                new("Eva", "Jeg elsker æbler", -46752800),
                new("John Doe", "I'm 6', dont bother me" , 72350352)
            };
            UserInterface.PrintCheeps(cheeps);

            string result = sw.ToString();
            Assert.Contains("Eva", result);
            Assert.Contains("Jeg elsker æbler", result);
            Assert.Contains("John Doe", result);
            Assert.Contains("I'm 6', dont bother me", result);
        }
    }
}