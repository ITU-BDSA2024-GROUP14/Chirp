using Chirp.Web.Pages.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.ViewComponents;

public class CheepTimelineViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(TimelineModel model)
    {
        return View("CheepTimeline");
    }
}