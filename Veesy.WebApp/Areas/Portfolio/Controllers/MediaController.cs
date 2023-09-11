using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Fluent;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Model.Media;
using Veesy.WebApp.Areas.Auth.Controllers;
using Veesy.WebApp.CustomDataAttribute;

namespace Veesy.WebApp.Areas.Portfolio.Controllers;

[Area("Portfolio")]
[Authorize]
public class MediaController : VeesyController
{

    private readonly MediaHelper _mediaHelper;
    private readonly IWebHostEnvironment _environment;
    private readonly ProfileHelper _profileHelper;
    private readonly INotyfService _notyfService;
    
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public MediaController(MediaHelper mediaHelper, UserManager<MyUser> userManager, IWebHostEnvironment environment, ProfileHelper profileHelper, INotyfService notyfService) : base(userManager)
    {
        _mediaHelper = mediaHelper;
        _environment = environment;
        _profileHelper = profileHelper;
        _notyfService = notyfService;
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
            await _mediaHelper.UploadFileAsync(HttpContext.Request.Body, Request.ContentType, UserInfo);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return View();
        }
    }
    
    
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [MultipartFormData]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> UpdateProfileImage()
    {
        try
        {
            var result = await _profileHelper.UpdateProfileImage(HttpContext.Request.Body, Request.ContentType, UserInfo);
            if(result.Success)
                _notyfService.Success("Image update correctly");
            else
                _notyfService.Error(result.Message);
            return RedirectToAction("BasicInfo", "Profile", new {area = "Account"});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            _notyfService.Error("Error during upload image. Please retry.");
            return RedirectToAction("BasicInfo", "Profile", new {area = "Account"});
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
