using Chirp.Core.DataModel;

namespace Chirp.Infrastructure.Repositories;

public interface ICheepRepository
{
    public IEnumerable<Cheep> GetCheepsByAuthor(List<string> authorUsernameList, int skip, int? size);
    public IEnumerable<Cheep> GetCheeps(int skip, int? size);
    public OriginalCheep CreateCheep(Author author, string text, DateTime timestamp);
    public RepostCheep CreateReCheep(Author author, OriginalCheep originalCheep, DateTime timestamp);
    public OriginalCheep GetOriginalCheepById(int cheepId);
}