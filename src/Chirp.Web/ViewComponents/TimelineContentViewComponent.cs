using Chirp.Web.Pages.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.ViewComponents;

/// <summary>
///     ViewComponent containing the timeline content component
/// </summary>
public class TimelineContentViewComponent : ViewComponent
{
    /// <summary>
    ///     Returns the Timeline content (Cheeps, and author name with buttons)
    /// </summary>
    /// <param name="model">The model to connect the timeline content to</param>
    /// <returns>IViewComponentResult containing the Timeline content</returns>
    public IViewComponentResult Invoke(TimelineModel model)
    {
        return View("TimelineContent");
    }
}