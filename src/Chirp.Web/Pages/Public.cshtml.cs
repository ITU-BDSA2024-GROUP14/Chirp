using System.ComponentModel.DataAnnotations;
using Chirp.Core.DataModel;
using Chirp.Infrastructure.Data.DataTransferObjects;
using Chirp.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly IChirpService _service;

    [BindProperty]
    [Required]
    [StringLength(Cheep.MaxLength, ErrorMessage = "Maximum length is {1}")]
    [Display(Name = "Cheep Text")]
    public string Message { get; set; }

    public List<CheepDTO> Cheeps { get; set; } = [];

    public PublicModel(IChirpService service)
    {
        _service = service;
        Message = "";
    }

    public IActionResult OnPostFollow(string toFollowAuthorName)
    {
        //await FollowUserAsync(toFollowAuthorName);
        if (!CheckIfFollowing(toFollowAuthorName))
        {
            FollowUser(toFollowAuthorName);
        }
        else
        {
            UnFollowUser(toFollowAuthorName);
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

    private string GetLoggedInBeak()
    {
        var userBeak = User.Claims.FirstOrDefault(claim => claim.Type == "Beak")?.Value;
        if (userBeak == null)
        {
            throw new NullReferenceException("Can't follow user since the logged in user does not exist.");
        }

        return userBeak;
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

    public ActionResult OnGet([FromQuery] int page = 1)
    {
        Cheeps = _service.GetCheeps(page);
        return Page();
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
}