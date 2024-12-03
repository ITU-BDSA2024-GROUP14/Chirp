using Chirp.Web.Pages.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.ViewComponents;

public class PaginationButtonsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(TimelineModel model)
    {
        return View("PaginationButtons");
    }
}