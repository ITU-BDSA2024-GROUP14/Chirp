using System.ComponentModel.DataAnnotations;
using Chirp.Core.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages.Shared.Models;

public class TimelineModel : PageModel
{
    public TimelineModel()
    {
        Message = "";
    }

    [BindProperty]
    [Required]
    [StringLength(Cheep.MaxLength, ErrorMessage = "Maximum length is {1}")]
    [Display(Name = "Cheep Text")]
    public string Message { get; set; }

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