namespace Chirp.Core;

public interface IDatabase
{
    public string ConnectionString { get; }
    
    public void EnsureCreated();
    
    public void Reset();
}