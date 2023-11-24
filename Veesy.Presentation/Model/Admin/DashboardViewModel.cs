using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Admin;

public class DashboardViewModel
{
    public List<MediaOverviewDto> MediaOverviewDtos { get; set; }
    public List<LastMediaUpload> LastMediaUploadDtos { get; set; }
    public List<int> SelectableMonth = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
}
