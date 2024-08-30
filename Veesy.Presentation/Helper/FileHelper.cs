using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Veesy.Domain.Constants;
using Veesy.Service.Dtos;
using Veesy.Service.Interfaces;

namespace Veesy.Presentation.Helper;

public class FileHelper
{
    private readonly IAccountService _accountService;
    private readonly IAnalyticService _analyticService;

    public FileHelper(IAccountService accountService, IAnalyticService analyticService)
    {
        _accountService = accountService;
        _analyticService = analyticService;
    }

    public (Stream file, string contentType, string name) GetCreatorsReport()
    {
        var users = _accountService.GetCreators();
        var creators = MapAdminDto.MapCreatorDtos(users);
        IWorkbook workbook = new XSSFWorkbook();

        ISheet sheet = workbook.CreateSheet($"Creators");

        var rowNumber = -1;
        var columnNumber = -1;
        IRow headerRow = sheet.CreateRow(++rowNumber);
        headerRow.CreateCell(++columnNumber).SetCellValue("NOME");
        headerRow.CreateCell(++columnNumber).SetCellValue("EMAIL");
        headerRow.CreateCell(++columnNumber).SetCellValue("TELEFONO");
        headerRow.CreateCell(++columnNumber).SetCellValue("RUOLO");
        headerRow.CreateCell(++columnNumber).SetCellValue("N° PORTFOLIO");
        headerRow.CreateCell(++columnNumber).SetCellValue("N° MEDIA");
        headerRow.CreateCell(++columnNumber).SetCellValue("DATA SOTTOSCRIZIONE");
        headerRow.CreateCell(++columnNumber).SetCellValue("SOTTOSCRIZIONE");
        headerRow.CreateCell(++columnNumber).SetCellValue("FIELDS");
        headerRow.CreateCell(++columnNumber).SetCellValue("SOFTWARE");
        headerRow.CreateCell(++columnNumber).SetCellValue("SOFT SKILL");

        // Aggiungi dati
        foreach(var creator in creators)
        {
            columnNumber = -1;
            IRow row = sheet.CreateRow(++rowNumber);
            row.CreateCell(++columnNumber).SetCellValue(creator.Fullname);
            row.CreateCell(++columnNumber).SetCellValue(creator.Email);
            row.CreateCell(++columnNumber).SetCellValue(creator.PhoneNumber);
            row.CreateCell(++columnNumber).SetCellValue(creator.Category);
            row.CreateCell(++columnNumber).SetCellValue(creator.PortfoliosCount);
            row.CreateCell(++columnNumber).SetCellValue(creator.MediasCount);
            row.CreateCell(++columnNumber).SetCellValue(creator.CreateDate);
            row.CreateCell(++columnNumber).SetCellValue(creator.SubscriptionPlan);
            var fields = "";
            if (creator.Fields.Count != 0)
                fields = creator.Fields.Aggregate((x, y) => x + ", " + y);
            row.CreateCell(++columnNumber).SetCellValue(fields);
            var software = "";
            if (creator.Software.Count != 0)
                software = creator.Software.Aggregate((x, y) => x + ", " + y);
            row.CreateCell(++columnNumber).SetCellValue(software);
            var softSkill = "";
            if (creator.SoftSkill.Count != 0)
                softSkill = creator.SoftSkill.Aggregate((x, y) => x + ", " + y);
            row.CreateCell(++columnNumber).SetCellValue(softSkill);
        }
        
        var memoryStream = new MemoryStream();
        workbook.Write(memoryStream, true);
        memoryStream.Position = 0;
        return (memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Resoconto_Creators_{DateTime.Now.ToString("ddMMyyyyHHmm")}.xlsx");
        
    }
    
    public (Stream file, string contentType, string name) GetTrackingLinkReport()
    {
        var analytics = _analyticService.GetReferralLinkTrackings();
        IWorkbook workbook = new XSSFWorkbook();

        ISheet sheet = workbook.CreateSheet($"Creators");

        var rowNumber = -1;
        var columnNumber = -1;
        IRow headerRow = sheet.CreateRow(++rowNumber);
        headerRow.CreateCell(++columnNumber).SetCellValue("LINK");
        headerRow.CreateCell(++columnNumber).SetCellValue("IP");
        headerRow.CreateCell(++columnNumber).SetCellValue("REFERER");
        headerRow.CreateCell(++columnNumber).SetCellValue("DATA");
        headerRow.CreateCell(++columnNumber).SetCellValue("BROWSER");
        headerRow.CreateCell(++columnNumber).SetCellValue("DEVICE");

        // Aggiungi dati
        foreach(var tracking in analytics)
        {
            columnNumber = -1;
            IRow row = sheet.CreateRow(++rowNumber);
            if (tracking.ReferralLink != null)
            {
                row.CreateCell(++columnNumber).SetCellValue(tracking.ReferralLink.Endpoint);
            }
            else
            {
                row.CreateCell(++columnNumber).SetCellValue("Page 404");
            }
            row.CreateCell(++columnNumber).SetCellValue(tracking.Ip);
            row.CreateCell(++columnNumber).SetCellValue(tracking.Referer);
            row.CreateCell(++columnNumber).SetCellValue(tracking.LastAccess.ToString("dd/MM/yyyy HH:mm"));
            row.CreateCell(++columnNumber).SetCellValue(tracking.Browser);
            row.CreateCell(++columnNumber).SetCellValue(@VeesyConstants.DeviceTypeText[tracking.DeviceType]);
        }
        
        var memoryStream = new MemoryStream();
        workbook.Write(memoryStream, true);
        memoryStream.Position = 0;
        return (memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Resoconto_ReferralLink_{DateTime.Now.ToString("ddMMyyyyHHmm")}.xlsx");
        
    }
}