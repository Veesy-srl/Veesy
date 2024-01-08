using Microsoft.AspNetCore.Mvc;

namespace Veesy.WebApp.Areas;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HandleErrorCode(int statusCode)
    {
        switch (statusCode)
        {
            case 404:
                // Restituisci la vista 404 personalizzata
                return RedirectToAction("Error404", "Public", new {area = "Public"});
            default:
                return RedirectToAction("Error400", "Public", new {area = "Public"});
        }
    }
}