using System.Diagnostics;
using SimpleDB;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace EndToEndTests;

public class EndToEnd
{
    private void SetUpTestDataBase(string fileName)
    {
        var content =
            "Author,Message,Timestamp\n" +
            "ropf,\"Hello, BDSA students!\",1690891760\n" +
            "adho,\"Welcome to the course!\",1690978778\n";
        using (var writer = new StreamWriter(fileName))
        {
            writer.WriteLine(content);
        }

        CheepDatabase.Instance.ChangeCsvPath(fileName);
    }

    private void PullDownTestDataBase(string fileName)
    {
        File.Delete(fileName);
    }


    //Inspiration taken from https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_03/Slides.md#end-to-end-e2e-tests-of-arbitrary-cli-programs
    [Theory]
    [InlineData("Test")]
    [InlineData("æøå")]
    public void TestCheep(string cheepMessage)
    {
        string output;
        var filePrefix = "../../../../../data/";
        var filename = "TestCheepEnd2End.csv";
        SetUpTestDataBase(filePrefix + filename);
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.WorkingDirectory = "../../../../../";
            process.StartInfo.Arguments = "./src/Chirp.CLI/bin/Debug/net8.0/Chirp.CLI.dll cheep " + cheepMessage
                + " -d ./data/" + filename;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            var reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }

        var cheep = CheepDatabase.Instance.Read(1).Last();


        Assert.Equal(cheepMessage, cheep.Message);

        PullDownTestDataBase(filePrefix + filename);
    }

    [Theory]
    [InlineData(1, "adho", "Welcome to the course!", 0)]
    [InlineData(2, "ropf", "Hello, BDSA students!", 0)]
    public void TestReadCheeps(int limit, string author, string message, int cheepNumber)
    {
        string output;
        var filePrefix = "../../../../../data/";
        var filename = "TestReadCheepsEnd2End.csv";
        SetUpTestDataBase(filePrefix + filename);
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.WorkingDirectory = "../../../../../";
            process.StartInfo.Arguments = "./src/Chirp.CLI/bin/Debug/net8.0/Chirp.CLI.dll read " + limit +
                                          " -d ./data/" + filename;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            var reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }

        var outputStrings = output.Split("\n");

        Assert.StartsWith(author, outputStrings[cheepNumber]);
        Assert.Contains(message, outputStrings[cheepNumber]);

        PullDownTestDataBase(filePrefix + filename);
    }
}