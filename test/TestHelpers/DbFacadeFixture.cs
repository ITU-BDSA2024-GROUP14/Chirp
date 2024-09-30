using Chirp.Core;

namespace TestHelpers;

public class DbFacadeFixture
{
    public IDatabase Db { get; private set; }
    public DBFacade DbFacade { get; private set; }

    public DbFacadeFixture()
    {
        Db = new TestDatabase();
        DbFacade = new DBFacade(Db);
    }
}