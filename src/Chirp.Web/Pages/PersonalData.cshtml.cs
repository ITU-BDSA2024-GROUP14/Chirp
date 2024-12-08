// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Chirp.Core.DataModel;
using Chirp.Infrastructure.Data.DataTransferObjects;
using Chirp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Chirp.Web.Areas.Identity.Pages;

public class PersonalDataModel : PageModel
{
    private readonly UserManager<Author> _userManager;
    private readonly SignInManager<Author> _signInManager;
    private readonly ILogger<PersonalDataModel> _logger;
    private readonly IChirpService _service;
    public AuthorDTO? Author;

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
            throw new InvalidOperationException($"Unexpected error occurred deleting user.");
        }

        await _signInManager.SignOutAsync();
        return Redirect("~/");
    }
}