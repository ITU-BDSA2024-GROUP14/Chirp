using System.Globalization;

using SimpleDB;

namespace Chirp.CLI.Tests;

public class UserInterfaceTests
{
    [Theory]
    [InlineData("Jens Petersen", "Min ko den har horn", -446752800)]
    public void PrintCheepContainsContent(string name, string content, long timeStamp)
    {
        using (StringWriter sw = new())
        {
            Console.SetOut(sw);

            Cheep cheep = new(name, content, timeStamp);
            UserInterface.PrintCheep(cheep);

            string result = sw.ToString();
            Assert.Contains(name, result);
            Assert.Contains(content, result);
            Assert.Contains(
                DateTimeOffset.FromUnixTimeSeconds(timeStamp).UtcDateTime
                    .ToString("dd/MM/yy HH:mm:ss", new CultureInfo("en-DE")),
                result);
        }
    }

    [Theory]
    [InlineData(new string[] { "Eva", "John Doe" }, new string[] { "Jeg elsker æbler", "I'm 6', dont bother me" },
        new long[] { -46752800, 72350352 })]
    public void PrintCheepsContainsContent(string[] names, string[] messages, long[] timeStamps)
    {
        Assert.True(names.Length == messages.Length, "Input must have same length");
        Assert.True(names.Length == timeStamps.Length, "Input must have same length");

        using (StringWriter sw = new())
        {
            Console.SetOut(sw);

            Cheep[] cheeps = new Cheep[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                cheeps[i] = new Cheep(names[i], messages[i], timeStamps[i]);
            }

            UserInterface.PrintCheeps(cheeps);

            string result = sw.ToString();

            for (int i = 0; i < names.Length; i++)
            {
                Assert.Contains(names[i], result);
                Assert.Contains(messages[i], result);
                Assert.Contains(
                    DateTimeOffset.FromUnixTimeSeconds(timeStamps[i]).UtcDateTime
                        .ToString("dd/MM/yy HH:mm:ss", new CultureInfo("en-DE")),
                    result);
            }
        }
    }
}