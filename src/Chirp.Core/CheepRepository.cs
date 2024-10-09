using Chirp.Core.DataModel;
using Chirp.Core.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core;

public interface ICheepRepository
{
    public IEnumerable<Cheep> GetCheeps(int skip, int? size, string? authorUsername = null);
}

/// <summary>
/// A repository that accesses Cheeps from the database.
/// </summary>
public class CheepRepository : ICheepRepository
{
    private ChirpDBContext _dbcontext;

    public CheepRepository(ChirpDBContext context)
    {
        _dbcontext = context;
    }

    /// <summary>
    /// Method <c>GetCheeps</c> returns specified cheeps from the database.
    /// </summary>  
    /// <param name="skip">The number of Cheeps to skip.</param>
    /// <param name="size">The number of Cheeps to return.</param>
    /// <param name="authorUsername">If there is an author, this is the username of the author of the Cheeps to return.</param>
    /// <returns>Specified cheeps from the database.</returns>
    public IEnumerable<Cheep> GetCheeps(int skip = 0, int? size = null, string? authorUsername = null)
    {
        var query = _dbcontext.Cheeps.Include(cheep => cheep.Author).AsQueryable();
        if (authorUsername != null)
        {
            query = query.Where(Cheep => Cheep.Author.Name == authorUsername);
        }

        query = query.Skip(skip);

        if (size != null)
        {
            query = query.Take((int)size);
        }

        return query.ToList();
    }
}