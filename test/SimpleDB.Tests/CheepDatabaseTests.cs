using System.Globalization;

using CsvHelper;

namespace SimpleDB.Tests;

public class CheepDatabaseTests
{
    [Fact]
    public void testIfInstanceReturnsAnInstance()
    {
        // Arrange

        // Act
        CheepDatabase database = CheepDatabase.Instance;

        // Assert
        Assert.NotNull(database);
    }

    [Fact]
    public void testIfInstanceReturnsTheSameDatabaseTwice()
    {
        // Arrange
        CheepDatabase database = CheepDatabase.Instance;

        // Act
        CheepDatabase database2 = CheepDatabase.Instance;

        // Assert
        Assert.Equal(database, database2);
    }

    [Fact]
    public void testIfCanReadFromDatabase()
    {
        // Arrange
        CheepDatabase database = CheepDatabase.Instance;
        CheepDatabase.Instance.ChangeCsvPath("./../../../testCSVdatabase.csv");

        // Act
        IEnumerable<Cheep> cheeps = database.Read();
        Cheep cheep = cheeps.First();

        // Assert
        Assert.Equal(cheep.Message, "Hello, BDSA students!");
        Assert.Equal(cheep.Author, "ropf");
        Assert.Equal(cheep.Timestamp, 1690891760);
    }

    [Fact]
    public void testIfCanWriteToDatabase()
    {
        // Arrange

        CheepDatabase database = CheepDatabase.Instance;
        string DatabasePath = "./../../../testCSVdatabase.csv";
        CheepDatabase.Instance.ChangeCsvPath(DatabasePath);
        IEnumerable<Cheep> cheeps;
        int beforeCount;
        using (StreamReader reader = new(DatabasePath))
        {
            using CsvReader csvReader = new(reader, new CultureInfo("en-DE"));
            cheeps = csvReader.GetRecords<Cheep>();
            beforeCount = cheeps.ToList().Count;
        }


        // Act
        Cheep cheep = new Cheep("aubu", "This is a test", 1111111111);
        database.Store(cheep);

        int afterCount;
        using (StreamReader reader = new(DatabasePath))
        {
            using CsvReader csvReader = new(reader, new CultureInfo("en-DE"));
            cheeps = csvReader.GetRecords<Cheep>();
            afterCount = cheeps.ToList().Count;

        }

        // Assert
        Assert.Equal(beforeCount + 1, afterCount);
    }
}