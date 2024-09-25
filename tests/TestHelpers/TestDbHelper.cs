using Chirp.CSVDBService;

namespace TestHelpers;

public static class TestDbHelper
{
    public static void SeedTestData(CheepDatabase database)
    {
        database.Store(new Cheep("ropf", "Hello, BDSA students!", 1690891760));
        database.Store(new Cheep("jonv", "Hello, BDSA students!", 1690891760));
        database.Store(new Cheep("aubu", "Hello, BDSA students!", 1690891760));
    }

    public static void SetupDatabase(CheepDatabase database)
    {
        database.DatabasePath = TestConstants.TestDbPath;
        database.Reset();
    }
}