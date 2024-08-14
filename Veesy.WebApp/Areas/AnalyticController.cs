using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Presentation.Helper;

namespace Veesy.WebApp.Areas;

public class AnalyticController : Controller
{
    private readonly AnalyticHelper _analyticHelper;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public AnalyticController(AnalyticHelper analyticHelper)
    {
        _analyticHelper = analyticHelper;
    }

    [HttpGet("referer/{endpoint}")]
    public async Task<IActionResult> Referral(string endpoint)
    {
        try 
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var referer = HttpContext.Request.Headers["Referer"].ToString();
            var latitudine = HttpContext.Request.Query["latitude"].ToString();
            var longitudine = HttpContext.Request.Query["longitude"].ToString();
            var ua = HttpContext.Request.Headers["User-Agent"].ToString();
            
            var url = await _analyticHelper.SaveReferral(endpoint, ip, referer, latitudine, longitudine, ua);
            
            if(string.IsNullOrEmpty(url))
                return RedirectToAction("Error404", "Public", new {area = "Public"});
            
            return Redirect(url);
        }
        catch (Exception e) 
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Error404", "Public", new {area = "Public"});
        }
    }
}