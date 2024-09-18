using System.Globalization;

using CsvHelper;

namespace SimpleDB.Tests;

public class CheepDatabaseTests
{
    [Fact]
    public void TestIfInstanceReturnsAnInstance()
    {
        // Arrange

        // Act
        CheepDatabase database = CheepDatabase.Instance;

        // Assert
        Assert.NotNull(database);
    }

    [Fact]
    public void TestIfInstanceReturnsTheSameDatabaseTwice()
    {
        // Arrange
        CheepDatabase database = CheepDatabase.Instance;

        // Act
        CheepDatabase database2 = CheepDatabase.Instance;

        // Assert
        Assert.Equal(database, database2);
    }

    [Fact]
    public void TestIfCanReadFromDatabase()
    {
        // Arrange
        CheepDatabase database = CheepDatabase.Instance;
        CheepDatabase.Instance.ChangeCsvPath("./../../../testCSVdatabase.csv");

        // Act
        IEnumerable<Cheep> cheeps = database.Read();
        Cheep cheep = cheeps.First();

        // Assert
        Assert.Equal("Hello, BDSA students!", cheep.Message);
        Assert.Equal("ropf", cheep.Author);
        Assert.Equal(1690891760, cheep.Timestamp);
    }

    [Fact]
    public void TestIfCanWriteToDatabase()
    {
        // Arrange

        CheepDatabase database = CheepDatabase.Instance;
        string DatabasePath = "./../../../testCSVdatabase.csv";
        CheepDatabase.Instance.ChangeCsvPath(DatabasePath);
        int beforeCount;
        int afterCount;

        // Act
        using (StreamReader reader = new(DatabasePath))
        {
            using CsvReader csvReader = new(reader, new CultureInfo("en-DE"));
            beforeCount = csvReader.GetRecords<Cheep>().ToList().Count;
        }

        Cheep cheep = new("aubu", "This is a test", 1111111111);
        database.Store(cheep);

        using (StreamReader reader = new(DatabasePath))
        {
            using CsvReader csvReader = new(reader, new CultureInfo("en-DE"));
            afterCount = csvReader.GetRecords<Cheep>().ToList().Count;
        }

        // Assert
        Assert.Equal(beforeCount + 1, afterCount);
    }
}