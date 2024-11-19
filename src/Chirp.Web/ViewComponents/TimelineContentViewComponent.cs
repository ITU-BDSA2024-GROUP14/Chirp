using Chirp.Web.Pages.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.ViewComponents;

public class TimelineContentViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(TimelineModel model)
    {
        return View("TimelineContent");
    }
}