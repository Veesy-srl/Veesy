using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Fluent;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Model.Media;
using Veesy.WebApp.CustomDataAttribute;

namespace Veesy.WebApp.Areas.Portfolio.Controllers;

public class MediaController : Controller
{

    private readonly MediaHelper _mediaHelper;
    
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public MediaController(MediaHelper mediaHelper)
    {
        _mediaHelper = mediaHelper;
    }

    [HttpGet]
    public IActionResult UploadMedia(string a)
    {
        return View();
    }
    
    // [HttpPost("upload-stream-multipartreader")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [MultipartFormData]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> UploadMedia(/*UploadMediaViewModel model*/)
    {
        try
        {
            await _mediaHelper.UploadFileAsync(HttpContext.Request.Body, Request.ContentType);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return View();
        }
    }
}
