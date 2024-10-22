using Chirp.Core.DataModel;

namespace Chirp.Core;

public interface ICheepRepository
{
    public IEnumerable<Cheep> GetCheeps(int skip, int? size, string? authorUsername = null);
    public Cheep CreateCheep(Author author, string text, DateTime timestamp);
}