using System.ComponentModel.DataAnnotations;
using Chirp.Core.DataModel;
using Chirp.Infrastructure.Data.DataTransferObjects;
using Chirp.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IChirpService _service;
    public List<CheepDTO> Cheeps { get; set; } = [];

    [BindProperty] public string Message { get; set; }

    public UserTimelineModel(IChirpService service)
    {
        _service = service;
        Message = "";
    }

    public ActionResult OnGet(string authorName, [FromQuery] int page = 1)
    {
        Cheeps = _service.GetCheepsFromAuthor(authorName, page);
        return Page();
    }

    public ActionResult OnPost()
    {
        var authorName = User.Identity?.Name;
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