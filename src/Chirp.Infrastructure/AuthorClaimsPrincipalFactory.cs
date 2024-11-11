using System.Security.Claims;
using Chirp.Core.DataModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Chirp.Infrastructure;

public class AuthorClaimsPrincipalFactory : UserClaimsPrincipalFactory<Author, IdentityRole<int>>
{
    public AuthorClaimsPrincipalFactory(UserManager<Author> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
    {
    }

}