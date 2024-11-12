using System.Security.Claims;
using Chirp.Core.DataModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Chirp.Infrastructure;

//Class adapted from https://korzh.com/blog/aspnet-identity-store-user-data-in-claims/
public class AuthorClaimsPrincipalFactory : UserClaimsPrincipalFactory<Author, IdentityRole<int>>
{
    public AuthorClaimsPrincipalFactory(UserManager<Author> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Author user)
    {
       var identity = await base.GenerateClaimsAsync(user);
       identity.AddClaim(new Claim("Beak", user.Beak));
       return identity;
    }
}