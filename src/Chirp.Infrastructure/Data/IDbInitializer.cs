using Chirp.Core.DataModel;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure.Data;

public interface IDbInitializer
{
    /// <summary>
    /// Adds seed data to the database
    /// </summary>
    /// <param name="userManager">If supplied, adds two sample users (Helge and Adrian) using ASP.NET Identity UserManager</param>
    public void Seed(UserManager<Author>? userManager = null);
}