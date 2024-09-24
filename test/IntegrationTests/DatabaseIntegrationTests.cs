using System.Globalization;
using CsvHelper;
using SimpleDB;

namespace IntegrationTests;

public class DatabaseIntegrationTests
{
    private const string TestDbPath = "./chirp_cli_db.csv";

    [Theory]
    [InlineData("Alice", "Hello, World!", 1625097600)] // Example timestamp: 1st July 2021
    [InlineData("Bob", "This is a test.", 1625184000)] // Example timestamp: 2nd July 2021
    [InlineData("Charlie", "Sample message", 1625270400)] // Example timestamp: 3rd July 2021
    public void StoreAndReadCheep_ShouldReturnSame(string author, string message, long timestamp)
    {
        // Arrange
        ResetDatabase();
        var database = CheepDatabase.Instance;
        
        // Act
        database.Store(new Cheep(author, message, timestamp));
        var cheeps = database.Read().ToList();

        // Assert
        Assert.Single(cheeps);
        var cheep = cheeps.First();
        Assert.Equal(author, cheep.Author);
        Assert.Equal(message, cheep.Message);
        Assert.Equal(timestamp, cheep.Timestamp);
    }
    
    [Fact]
    public void StoreMultipleCheeps_ShouldStoreAll()
    {
        // Arrange
        var database = CheepDatabase.Instance;
        ResetDatabase();
        var cheepsToStore = new List<Cheep>
        {
            new("Alice", "Hello, World!", 1625097600),
            new("Bob", "This is a test.", 1625184000),
            new("Charlie", "Sample message", 1625270400)
        };

        // Act
        foreach (var cheep in cheepsToStore)
        {
            database.Store(cheep);
        }
        var cheeps = database.Read().ToList();

        // Assert
        Assert.Equal(cheepsToStore.Count, cheeps.Count);
        
        // Check that all cheeps that we wrote are present in the database
        foreach (var cheep in cheepsToStore)
        {
            Assert.Contains(cheeps, c => c.Author == cheep.Author && c.Message == cheep.Message && c.Timestamp == cheep.Timestamp);
        }
    }
    
    private void ResetDatabase()
    {
        var database = CheepDatabase.Instance;
        database.ChangeCsvPath(TestDbPath);
        File.WriteAllText(TestDbPath, string.Empty);
        using var stream = File.Open(TestDbPath, FileMode.Append);
        using StreamWriter writer = new(stream);
        using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
        csv.WriteHeader(typeof(Cheep));
        csv.NextRecord();
    }
}