using System.ComponentModel.DataAnnotations;
using Chirp.Core.DataModel;
using Chirp.Infrastructure.Data.DataTransferObjects;
using Chirp.Infrastructure.Services;
using Chirp.Web.Pages.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

/// <summary>
/// The model instance to be used for the private timeline
/// </summary>
public class UserTimelineModel(IChirpService service) : TimelineModel(service)
{
    /// <summary>
    /// Sets the cheeps to be displayed on the page
    /// Sets the current page number
    /// Sets the logged in display name if the user is logged in
    /// Sets the follow list
    /// </summary>
    /// <param name="authorName">The name of the owner of the timeline</param>
    /// <param name="page">The current page number, defaults to 1</param>
    /// <returns></returns>
    public ActionResult OnGet(string authorName, [FromQuery] int page = 1)
    {
        if (User.Identity is not { IsAuthenticated: true })
        {
            Cheeps = _service.GetCheepsFromAuthor(authorName, page);
            return Page();
        }
        var loggedInDisplayName = GetLoggedInDisplayName();

        if (loggedInDisplayName != authorName)
        {
            Cheeps = _service.GetCheepsFromAuthor(authorName, page);
            return Page();
        }
        base.SetPageNumber(page);
        var followList = _service.GetFollowing(loggedInDisplayName);
        followList.Add(authorName);
        Cheeps = _service.GetCheepsFromMultipleAuthors(followList);
        return Page();
    }

    /// <summary>
    /// Cheeps input in the cheepbox, form the logged in users name
    /// Redirects to the public timeline if the no one is logged in, or if the logged in user cannot be found in the database
    /// If everything goes perfectly redirects to the users private timeline
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