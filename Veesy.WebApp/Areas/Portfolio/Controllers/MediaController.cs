using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Fluent;
using NuGet.Protocol;
using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Model.Cloud;
using Veesy.Presentation.Model.Media;
using Veesy.Service.Dtos;
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
    private readonly IConfiguration _configuration;
    
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public MediaController(MediaHelper mediaHelper, UserManager<MyUser> userManager, IWebHostEnvironment environment, ProfileHelper profileHelper, INotyfService notyfService, IConfiguration configuration) : base(userManager, configuration)
    {
        _mediaHelper = mediaHelper;
        _environment = environment;
        _profileHelper = profileHelper;
        _notyfService = notyfService;
        _configuration = configuration;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [MultipartFormData]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> UploadMedia()
    {
        try
        {
            var result = await _mediaHelper.UploadFileAsync(HttpContext.Request.Body,  Request.ContentType, UserInfo);
            var successFiles = "Files upload: </br>";
            var errorFiles = "Files not upload: </br>";
            var response = new UploadMediaResponseDto();
            foreach (var res in result)
            {
                if (res.success)
                {
                    successFiles += res.fileName + "</br>";
                    response.NumberSuccessFile++;
                    response.MediaDtos.Add(res.media);
                }
                else
                {
                    errorFiles += res.fileName + " - " + res.message + "</br>";
                    response.NumberErrorFile++;
                }
            }

            response.SuccessFileMessage = successFiles;
            response.ErrorFileMessage = errorFiles;
            return Ok(response.ToJson());
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            _notyfService.Custom("Error during upload file. Please retry.", 10, "#ca0a0a");
            return BadRequest();
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteMedia(EditViewModel model)
    {
        try
        {
            var result = await _mediaHelper.DeleteFile(model.Media.Code, UserInfo);
            if (result.success)
            {
                _notyfService.Custom("Media delete correctly.", 10, "#75CCDD");
                if (result.previousMedia != null)
                    return RedirectToAction("Edit", "Cloud", new { area = "Portfolio", id = result.previousMedia.Id });
                return RedirectToAction("List", "Cloud", new { area = "Portfolio" });
            }
            _notyfService.Custom(result.message, 10, "#ca0a0a");
            return RedirectToAction("Edit", "Cloud", new { area = "Portfolio", id = result.previousMedia.Id });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            _notyfService.Custom("Error deleting file. Please retry.",10, "#ca0a0a");
            return RedirectToAction("List", "Cloud", new { area = "Portfolio" });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> DeleteMediaList([FromBody] List<Guid> imgToDelete)
    {
        try
        {
            var result = await _mediaHelper.DeleteFiles(imgToDelete, UserInfo);
            var successFiles = "Files delete: <br/>";
            var errorFiles = "Files not delete: <br/>";
            var codeToDelete = result.Select(s => s.code);
            foreach (var res in result)
            {
                if (res.success)
                    successFiles += res.filename + "<br/>";
                else
                    errorFiles += res.filename + " - " + res.message + "<br/>";
            }
            if (successFiles != "Files delete: <br/>")
                _notyfService.Custom(successFiles, 10, "#75CCDD");
            if (errorFiles != "Files not delete: <br/>")
                _notyfService.Custom(errorFiles, 10, "#ca0a0a");
            return Json(new { Result = true, SuccessFiles = successFiles, ErrorFiles = errorFiles, CodeToDelete = codeToDelete });
        }
        catch (Exception ex)
        {
            return Json(new { Result = false, Message = "Error deleting files. Please retry." });
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
                _notyfService.Custom("Image update correctly.", 10, "#75CCDD");
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
