using Microsoft.EntityFrameworkCore;

namespace Chirp.Core;

public interface ICheepRepository
{
    public IEnumerable<CheepRecord> GetCheeps(int page, string authorUsername);
}

public class CheepRepository : ICheepRepository
{
    private ChirpDBContext _dbcontext;

    public CheepRepository(ChirpDBContext context)
    {
        _dbcontext = context;
    }

    public IEnumerable<CheepRecord> GetCheeps(int page, string? authorUsername = null)
    {
        var query = from cheep in _dbcontext.Cheeps
            select cheep;
        if (authorUsername != null)
        {
            query = from cheep in query
                where cheep.Author.Name == authorUsername
                select cheep;
        }

        return query.ToListAsync();
    }
}