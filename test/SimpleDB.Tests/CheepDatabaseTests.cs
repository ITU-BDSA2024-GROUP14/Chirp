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
}