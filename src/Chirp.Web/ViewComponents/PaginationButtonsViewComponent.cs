using Chirp.Web.Pages.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.ViewComponents;

/// <summary>
/// ViewComponent containing the pagination buttons component
/// </summary>
public class PaginationButtonsViewComponent : ViewComponent
{
    /// <summary>
    /// Returns the pagination buttons, that allows for changing the page
    /// </summary>
    /// <param name="model">The model to connect the pagination buttons to</param>
    /// <returns>IViewComponentResult containing the pagination buttons</returns>
    public IViewComponentResult Invoke(TimelineModel model)
    {
        return View("PaginationButtons");
    }
}