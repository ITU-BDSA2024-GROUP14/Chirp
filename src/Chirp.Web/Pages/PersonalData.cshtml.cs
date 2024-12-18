// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Chirp.Core.DataModel;
using Chirp.Infrastructure.Data.DataTransferObjects;
using Chirp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Areas.Identity.Pages;

/// <summary>
///     Data model personal data page
/// </summary>
public class PersonalDataModel : PageModel
{
    private readonly ILogger<PersonalDataModel> _logger;
    private readonly IChirpService _service;
    private readonly SignInManager<Author> _signInManager;
    private readonly UserManager<Author> _userManager;
    public AuthorDTO? Author;

    /// <summary>
    ///     Sets needed fields for the class
    /// </summary>
    /// <param name="userManager">userManger of the page</param>
    /// <param name="signInManager">signInManager of the page</param>
    /// <param name="logger">logger of the page</param>
    /// <param name="service">The IChirpService to interact with the base from</param>
    public PersonalDataModel(
        UserManager<Author> userManager,
        SignInManager<Author> signInManager,
        ILogger<PersonalDataModel> logger,
        IChirpService service)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _service = service;
    }

    /// <summary>
    ///     Sets the logged in user
    ///     Sets the cheeps, follow list and name of the logged in author
    /// </summary>
    /// <returns>Task to be waited on, returns NotFound object if the logged in user cannot be found</returns>
    /// <exception cref="NullReferenceException"></exception>
    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var authorName = User.Claims.FirstOrDefault(claim => claim.Type == "DisplayName")?.Value;
        if (authorName != null)
        {
            Author = _service.GetAuthorByName(authorName) ?? throw new NullReferenceException();
            Author.Cheeps = _service.GetCheepsFromAuthor(authorName);
            Author.Following = _service.GetFollowing(authorName);
        }

        return Page();
    }

    //Method inspired by PersonalDataModel in the Identity scaffolded pages
    /// <summary>
    ///     Finds the logged in user and tries to delete the user
    ///     On success signs out the user, and redirects to the root page (Public timeline)
    /// </summary>
    /// <returns>Task to be waited on, if no user is logged in it returns a NotFound Task</returns>
    /// <exception cref="InvalidOperationException">Throws exception if it dosent successfully delete user</exception>
    public async Task<IActionResult> OnPostForgetUser()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Unexpected error occurred deleting user.");
        }

        await _signInManager.SignOutAsync();
        return Redirect("~/");
    }
}