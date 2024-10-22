using Chirp.Infrastructure.DataTransferObjects;
using Chirp.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IChirpService _service;
    public List<CheepDTO> Cheeps { get; set; } = [];

    public UserTimelineModel(IChirpService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author, [FromQuery] int page = 1)
    {
        Cheeps = _service.GetCheepsFromAuthor(author, page);
        return Page();
    }
}