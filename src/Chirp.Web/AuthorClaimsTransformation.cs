using System.Security.Claims;
using Chirp.Core.DataModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Web;

//Taken and edited from https://learn.microsoft.com/en-us/aspnet/core/security/authentication/claims?view=aspnetcore-8.0#extend-or-add-custom-claims-using-iclaimstransformation
public class AuthorClaimsTransformation : IClaimsTransformation
{
    private UserManager<Author> _userManager;

    /// <summary>
    /// Sets the _userManager of the authorClaimsTransformation
    /// </summary>
    /// <param name="_userManager">To user manager of the program</param>
    public AuthorClaimsTransformation(UserManager<Author> _userManager)
    {
        this._userManager = _userManager;
    }

    /// <summary>
    /// Creates a display name claim on the claims principal.
    /// </summary>
    /// <param name="principal">The ClaimsPrincipal to add the displayname claim to</param>
    /// <returns>A task that can be awaited</returns>
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var claimsIdentity = new ClaimsIdentity();
        const string claimType = "DisplayName";
        if (!principal.HasClaim(claim => claim.Type == claimType))
        {
            var user = await _userManager.GetUserAsync(principal);
            
            // If the user is in the middle of registering using an external login, the user is not yet created
            if (user != null)
            {
                claimsIdentity.AddClaim(new Claim(claimType, user.DisplayName));
            }
        }

        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}