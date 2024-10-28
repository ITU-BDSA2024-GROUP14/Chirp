using Chirp.Core.DataModel;

namespace Chirp.Infrastructure.Repositories;

public interface ICheepRepository
{
    public IEnumerable<Cheep> GetCheepsByAuthor(string author, int skip, int? size);
    public IEnumerable<Cheep> GetCheeps(int skip, int? size, string? authorUsername = null);
    public Cheep CreateCheep(Author author, string text, DateTime timestamp);
}