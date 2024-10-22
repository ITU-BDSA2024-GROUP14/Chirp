using Chirp.Infrastructure.DataTransferObjects;
using Chirp.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IChirpService _service;
    public List<CheepDTO> Cheeps { get; set; } = [];

    public PublicModel(IChirpService service)
    {
        _service = service;
    }

    public ActionResult OnGet([FromQuery] int page = 1)
    {
        Cheeps = _service.GetCheeps(page);
        return Page();
    }
}