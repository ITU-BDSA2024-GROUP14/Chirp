using Chirp.Web.Pages.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.ViewComponents;

public class TimelineViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(TimelineModel model, string timelineOwner)
    {
        ViewBag.TimelineOwner = timelineOwner;
        return View("Timeline");
    }
}