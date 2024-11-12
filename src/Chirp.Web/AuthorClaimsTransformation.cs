using System.Security.Claims;
using Chirp.Core.DataModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Web;

//Taken and edited from https://learn.microsoft.com/en-us/aspnet/core/security/authentication/claims?view=aspnetcore-8.0#extend-or-add-custom-claims-using-iclaimstransformation
public class AuthorClaimsTransformation : IClaimsTransformation
{
    private UserManager<Author> _userManager;

    public AuthorClaimsTransformation(UserManager<Author> _userManager)
    {
        this._userManager = _userManager;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var claimsIdentity = new ClaimsIdentity();
        var claimType = "Beak";
        if (!principal.HasClaim(claim => claim.Type == claimType))
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                throw new NullReferenceException("No user type found");
            }
            claimsIdentity.AddClaim(new Claim(claimType, user.Beak));
        }
        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}