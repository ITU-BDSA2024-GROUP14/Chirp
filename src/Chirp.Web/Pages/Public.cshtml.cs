using Chirp.Infrastructure.Services;
using Chirp.Web.Pages.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

/// <summary>
///     The model instance to be used for the public timeline
/// </summary>
public class PublicModel : TimelineModel
{
    /// <summary>
    ///     Sets the IChirpService that should be used to get data from
    /// </summary>
    /// <param name="service">The service to use</param>
    public PublicModel(IChirpService service) : base(service)
    {
    }

    /// <summary>
    ///     Sets the page number of the current page
    ///     Sets the list of cheeps to be displayed on the current page
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public ActionResult OnGet([FromQuery] int page = 1)
    {
        SetPageNumber(page);
        Cheeps = _service.GetCheeps(page);
        return Page();
    }

    /// <summary>
    ///     Cheeps input in the cheepbox, form the logged in users name
    ///     Redirects to the public timeline if the no one is logged in, or if the logged in user cannot be found in the
    ///     database
    ///     If everything goes perfectly redirects to the users private timeline
    /// </summary>
    /// <returns>Actionresult</returns>
    public ActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var authorName = User.Claims.FirstOrDefault(claim => claim.Type == "DisplayName")?.Value;

        if (authorName == null)
        {
            return RedirectToPage("./PublicTimeline");
        }

        var author = _service.GetAuthorByName(authorName);
        if (author == null)
        {
            return RedirectToPage("./PublicTimeline");
        }

        _service.CreateCheep(author.Name, author.Email, Message, DateTime.Now);
        return RedirectToPage("./UserTimeline", new { authorName });
    }
}