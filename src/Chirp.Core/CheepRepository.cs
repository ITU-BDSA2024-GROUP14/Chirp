using Chirp.Core.DataModel;
using Chirp.Core.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core;

public interface ICheepRepository
{
    public IEnumerable<Cheep> GetCheeps(int skip, int? size, string? authorUsername = null);
}

public class CheepRepository : ICheepRepository
{
    private ChirpDBContext _dbcontext;

    public CheepRepository(ChirpDBContext context)
    {
        _dbcontext = context;
    }

    public IEnumerable<Cheep> GetCheeps(int skip = 0, int? size = null, string? authorUsername = null)
    {
        var query = _dbcontext.Cheeps.AsQueryable();
        if (authorUsername != null)
        {
            query = query.Where(Cheep => Cheep.Author.Name == authorUsername);
        }

        query = query.Skip(skip);

        if (size != null)
        {
            query = query.Take((int) size);
        }

        return query.ToList();
    }
}