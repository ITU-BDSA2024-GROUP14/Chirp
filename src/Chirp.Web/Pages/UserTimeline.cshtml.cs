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

    [BindProperty]
    [Required]
    [StringLength(Cheep.MaxLength, ErrorMessage = "Maximum length is {1}")]
    [Display(Name = "Cheep Text")]
    public string Message { get; set; }

    public UserTimelineModel(IChirpService service)
    {
        _service = service;
        Message = "";
    }

    public ActionResult OnGet(string authorName, [FromQuery] int page = 1)
    {
        if (User.Identity is not { IsAuthenticated: true })
        {
            Cheeps = _service.GetCheepsFromAuthor(authorName, page);
            return Page();
        }
        var loggedInBeak = GetLoggedInBeak();

        if (loggedInBeak != authorName)
        {
            Cheeps = _service.GetCheepsFromAuthor(authorName, page);
            return Page();
        }
        var followList = _service.GetFollowing(loggedInBeak);
        followList.Add(authorName);
        Cheeps = _service.GetCheepsFromMultipleAuthors(followList);
        return Page();
    }

    public IActionResult OnPostFlipFollow(string authorName)
    {
        //await FollowUserAsync(toFollowAuthorName);
        if (!CheckIfFollowing(authorName))
        {
            FollowUser(authorName);
        }
        else
        {
            UnFollowUser(authorName);
        }
        return RedirectToPage();
    }

    private void FollowUser(string toFollowAuthorName)
    {
        var authorName = GetLoggedInBeak();
        _service.FollowUser(authorName, toFollowAuthorName);
    }

    private void UnFollowUser(string toUnFollowAuthorName)
    {
        var authorName = GetLoggedInBeak();
        _service.UnFollowUser(authorName, toUnFollowAuthorName);
    }

    public bool CheckIfFollowing(string followingAuthorName)
    {
        var authorName = User.Claims.FirstOrDefault(claim => claim.Type == "Beak")?.Value;
        if (authorName == null)
        {
            throw new NullReferenceException("Can't follow user since the logged in user does not exist.");
        }

        return _service.CheckIfFollowing(authorName, followingAuthorName);
    }

    public ActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var authorName = User.Claims.FirstOrDefault(claim => claim.Type == "Beak")?.Value;
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

    public string GetLoggedInBeak()
    {
        var userBeak = User.Claims.FirstOrDefault(claim => claim.Type == "Beak")?.Value;
        if (userBeak == null)
        {
            throw new NullReferenceException("Can't get logged in user name, since there is no user logged in");
        }

        return userBeak;
    }
}