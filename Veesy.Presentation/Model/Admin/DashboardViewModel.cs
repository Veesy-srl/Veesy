using System.Collections.Generic;
using Veesy.Domain.Models;
using Veesy.Domain.Models.Log;
using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Admin;

public class DashboardViewModel
{
    public List<MediaOverviewDto> MediaOverviewDtos { get; set; }
    public List<LastMediaUpload> LastMediaUploadDtos { get; set; }
    public int CreatorNumber { get; set; }
    public int MediaNumber { get; set; }
    public List<CreatorOverviewDto> CreatorOverviewDtos { get; set; }
    public int NumberPayingUsers { get; set; }
    public List<MyUser> LastUsersCreated { get; set; }

    public List<int> SelectableMonth = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
    public (int DraftPortfolios, int PublishedPortfolios) PortfoliosCount { get; set; }
    public List<MapOverviewDto> MapOverviewDtos { get; set; }
    public List<UserSecurity> LastAccess { get; set; }
}
