using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veesy.Domain.Constants;
using NLog;
using Veesy.Presentation.Helper;

namespace Veesy.WebApp.Areas;

public class FileController : Controller
{
    private readonly FileHelper _fileHelper;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public FileController(FileHelper fileHelper)
    {
        _fileHelper = fileHelper;
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpGet("admin/export-excel-creator")]
    public FileResult ExportExcelCreator()
    {
        try
        {
            var result = _fileHelper.GetCreatorsReport();
            return File(result.file, result.contentType, result.name);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            throw;
        }
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpGet("admin/export-excel-referral-analytics")]
    public FileResult ExportExcelReferralAnalytics()
    {
        try
        {
            var result = _fileHelper.GetTrackingLinkReport();
            return File(result.file, result.contentType, result.name);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            throw;
        }
    }
}