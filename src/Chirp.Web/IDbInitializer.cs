using Chirp.Core.DataModel;
using Chirp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Web;

public interface IDbInitializer
{
    public void SeedDatabase(ChirpDBContext chirpContext, UserManager<Author> userManager);
}