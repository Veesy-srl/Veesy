using Veesy.Presentation.Helper;

namespace Veesy.Presentation.Model.Admin;

public class SubscriptionOverviewViewModel
{
    public decimal EarningsThisMonth { get; set; } 
    public decimal EarningsThisYear { get; set; }
    public int NumberPayingUsers { get; set; }
    public List<AdminHelper.EarningYearGroupedByMonthDto> EarningGraph { get; set; }
}