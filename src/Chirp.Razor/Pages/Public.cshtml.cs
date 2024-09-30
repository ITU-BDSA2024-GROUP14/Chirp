using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet()
    {
        var pageString = Request.Query["page"].ToString();
        var page = 1;
        if (pageString != "")
        {
            page = int.Parse(pageString);
        }
        Cheeps = _service.GetCheeps(page);
        return Page();
    }
}