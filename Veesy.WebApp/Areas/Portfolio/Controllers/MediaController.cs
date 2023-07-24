using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Fluent;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Model.Media;
using Veesy.WebApp.CustomDataAttribute;

namespace Veesy.WebApp.Areas.Portfolio.Controllers;

[Area("Portfolio")]
public class MediaController : Controller
{

    private readonly MediaHelper _mediaHelper;
    private readonly IWebHostEnvironment _environment;
    
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public MediaController(MediaHelper mediaHelper, IWebHostEnvironment environment)
    {
        _mediaHelper = mediaHelper;
        _environment = environment;
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
    
    [HttpGet("media/{section}/{fileName}")]
    public async Task<FileResult> Get(string section, string fileName)
    {
        try
        {
            var image = await _mediaHelper.GetMediaFromBlob(section, fileName);
            return File(image.BlobContent, image.BlobContentType);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return File(await System.IO.File.ReadAllBytesAsync($"{_environment.WebRootPath}\\imgs\\notfound.jpg"), "image/jpeg");
        }
    }
}
