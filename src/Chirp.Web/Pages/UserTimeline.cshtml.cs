using System.ComponentModel.DataAnnotations;
using Chirp.Core.DataModel;
using Chirp.Infrastructure.Data.DataTransferObjects;
using Chirp.Infrastructure.Services;
using Chirp.Web.Pages.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel(IChirpService service) : TimelineModel(service)
{

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