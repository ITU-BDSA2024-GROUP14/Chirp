using System.ComponentModel.DataAnnotations;
using Chirp.Core.DataModel;
using Chirp.Infrastructure.Data.DataTransferObjects;
using Chirp.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages.Shared.Models;

public abstract class TimelineModel(IChirpService service) : PageModel
{

    protected readonly IChirpService _service = service;

    public List<CheepDTO> Cheeps { get; set; } = [];

    [BindProperty]
    [Required]
    [StringLength(Cheep.MaxLength, ErrorMessage = "Maximum length is {1}")]
    [Display(Name = "Cheep Text")]
    public string Message { get; set; } = "";

    public string GetLoggedInBeak()
    {
        var userBeak = User.Claims.FirstOrDefault(claim => claim.Type == "Beak")?.Value;
        if (userBeak == null)
        {
            throw new NullReferenceException("Can't get logged in user name, since there is no user logged in");
        }

        return userBeak;
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
}