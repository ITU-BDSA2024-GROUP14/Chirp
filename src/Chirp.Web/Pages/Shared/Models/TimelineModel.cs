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
    public int PageNumber = 1;

    public List<CheepDTO> Cheeps { get; set; } = [];

    [BindProperty]
    [Required]
    [StringLength(Cheep.MaxLength, ErrorMessage = "Maximum length is {1}")]
    [Display(Name = "Cheep Text")]
    public string Message { get; set; } = "";

    /// <summary>
    ///     Get the logged in users display name
    /// </summary>
    /// <returns>The logged in users display name</returns>
    /// <exception cref="NullReferenceException">Throws NullReferenceException if no logged in user can be found</exception>
    public string GetLoggedInDisplayName()
    {
        var userDisplayName = User.Claims.FirstOrDefault(claim => claim.Type == "DisplayName")?.Value;
        if (userDisplayName == null)
        {
            throw new NullReferenceException("Can't get logged in user name, since there is no user logged in");
        }

        return userDisplayName;
    }

    /// <summary>
    ///     Sets the current page number
    /// </summary>
    /// <param name="page">Current page number</param>
    public void SetPageNumber(int page)
    {
        PageNumber = page;
    }

    /// <summary>
    ///     Flips the following status of the given author. So if they are followed, it unfollows them, and if they are not
    ///     following them, it follows them.
    /// </summary>
    /// <param name="authorName">The author where the follow status should be flipped</param>
    /// <returns>IActionResult</returns>
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

    /// <summary>
    ///     Recheeps given cheepid
    /// </summary>
    /// <param name="originalCheepId">The cheep id to be recheeped</param>
    /// <returns>IActonResult</returns>
    public IActionResult OnPostReCheep(int originalCheepId)
    {
        _service.ReCheep(GetLoggedInDisplayName(), originalCheepId);

        var authorName = GetLoggedInDisplayName();
        return RedirectToPage("./UserTimeline", new { authorName });
    }

    /// <summary>
    ///     Makes logged in user follow given author
    /// </summary>
    /// <param name="toFollowAuthorName">Author to be followed by logged in user</param>
    private void FollowUser(string toFollowAuthorName)
    {
        var authorName = GetLoggedInDisplayName();
        _service.FollowUser(authorName, toFollowAuthorName);
    }

    /// <summary>
    ///     Makes logged in user unfollow given author name
    /// </summary>
    /// <param name="toUnFollowAuthorName">Author to unfollow</param>
    private void UnFollowUser(string toUnFollowAuthorName)
    {
        var authorName = GetLoggedInDisplayName();
        _service.UnFollowUser(authorName, toUnFollowAuthorName);
    }

    /// <summary>
    ///     The following method checks if the logged in user is following the giving authorname
    /// </summary>
    /// <param name="followingAuthorName">The author that might be followed</param>
    /// <returns>True if the logged in user follows the given author, returns false otherwise</returns>
    /// <exception cref="NullReferenceException">If the logged in user dosent exist</exception>
    public bool CheckIfFollowing(string followingAuthorName)
    {
        var authorName = User.Claims.FirstOrDefault(claim => claim.Type == "DisplayName")?.Value;
        if (authorName == null)
        {
            throw new NullReferenceException("Can't follow user since the logged in user does not exist.");
        }

        return _service.CheckIfFollowing(authorName, followingAuthorName);
    }
}