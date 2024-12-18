using Chirp.Web.Pages.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.ViewComponents;

/// <summary>
///     ViewComponent containing the cheepbox component
/// </summary>
public class CheepBoxViewComponent : ViewComponent
{
    /// <summary>
    ///     Returns the cheepbox, that allows for sending cheeps
    /// </summary>
    /// <param name="model">The model to connect the cheepbox to</param>
    /// <returns>IViewComponentResult containing cheepbox</returns>
    public IViewComponentResult Invoke(TimelineModel model)
    {
        return View("CheepBox");
    }
}