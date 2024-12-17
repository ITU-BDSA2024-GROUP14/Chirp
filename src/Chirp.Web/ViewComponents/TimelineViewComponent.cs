using Chirp.Web.Pages.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.ViewComponents;

/// <summary>
/// ViewComponent containing the timeline
/// </summary>
public class TimelineViewComponent : ViewComponent
{
    /// <summary>
    /// Returns the timeline, weather public or private, that has all of the needed things in it, such as cheepbox, pagination buttons, and the timeline content
    /// </summary>
    /// <param name="model">The model to connect the timeline to</param>
    /// /// <param name="timelineOwner">The owner of the timeline, should either be public or the name of a user</param>
    /// <returns>IViewComponentResult containing the timeline</returns>
    public IViewComponentResult Invoke(TimelineModel model, string timelineOwner)
    {
        ViewBag.TimelineOwner = timelineOwner;
        return View("Timeline");
    }
}