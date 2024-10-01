namespace Chirp.Core;

public interface IDatabase
{
    public string ConnectionString { get; }

    public void EnsureCreated();

    /// <summary>
    /// Clears the database and recreates the schema
    /// </summary>
    public void Reset();
}