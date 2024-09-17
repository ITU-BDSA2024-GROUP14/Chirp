using System.Diagnostics;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace EndToEndTests;

public class EndToEnd
{
    private void SetUpTestDataBase(string fileName)
    {
        var content =
            "Author,Message,Timestamp\nropf,\"Hello, BDSA students!\",1690891760\nadho,\"Welcome to the course!\",1690978778\nadho,\"I hope you had a good summer.\",1690979858\nropf,\"Cheeping cheeps on Chirp :)\",1690981487\nostarup,\"Hello world\",1724853505\nostarup,\",,,,ffdsfds,,,,s,\",1724853546\nostarup,\",,,,ffdsfds,,,D,Ds,\",1724853572\nostarup,\",,,,ffdsfds,,,D,Ds,\",1724853698\ndanielh,Hej med dig,1725458581\ndanielh,Sommeren er slut,1725864411\ndanielh,dette er en test,1725864926\ndanielh,dette er en test222222,1725864961\naugus,yoyoyo,1726062158";
        using (var writer = new StreamWriter(fileName))
        {
            writer.WriteLine(content);
        }
    }

    private void PullDownTestDataBase(string fileName)
    {
        File.Delete(fileName);
    }


    //Inspiration taken from https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_03/Slides.md#end-to-end-e2e-tests-of-arbitrary-cli-programs
    [Theory]
    [InlineData("Test")]
    [InlineData("æøå")]
    [InlineData(",2,5,dsadsad, ss\"")]
    public void TestCheep(string cheepMessage)
    {
        var filename = "TestCheepEnd2End.txt";
        SetUpTestDataBase(filename);
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.WorkingDirectory = "../../../../../";
            process.StartInfo.Arguments = "./src/Chirp.CLI/bin/Debug/net8.0/Chirp.CLI.dll cheep " + cheepMessage;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            var reader = process.StandardOutput;
            var output = reader.ReadToEnd();
            process.WaitForExit();
        }

        var cheep = File.ReadLines(filename).Last();


        Assert.Equal(cheepMessage, cheep.Substring(0, cheepMessage.Length));

        PullDownTestDataBase(filename);
    }
}