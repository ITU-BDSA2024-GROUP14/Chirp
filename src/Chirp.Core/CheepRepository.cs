using Chirp.Core.DataModel;
using Chirp.Core.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core;

public interface ICheepRepository
{
    public IEnumerable<Cheep> GetCheeps(int skip, int? size, string? authorUsername = null);
    public Cheep CreateCheep(Author author, string text, DateTime timestamp);
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
        var query = _dbcontext.Cheeps.Include(cheep => cheep.Author).AsQueryable();
        if (authorUsername != null)
        {
            query = query.Where(Cheep => Cheep.Author.Name == authorUsername);
        }

        query = query.OrderByDescending(cheep => cheep.TimeStamp);

        query = query.Skip(skip);

        if (size != null)
        {
            query = query.Take((int)size);
        }

        return query.ToList();
    }

    public Cheep CreateCheep(Author author, string text, DateTime timestamp)
    {
        var cheep = new Cheep { Author = author, Text = text, TimeStamp = timestamp };
        _dbcontext.Cheeps.Add(cheep);
        _dbcontext.SaveChanges();
        return cheep;
    }
}