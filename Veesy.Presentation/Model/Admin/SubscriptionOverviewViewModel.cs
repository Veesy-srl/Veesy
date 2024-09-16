using System.Collections.Generic;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;

namespace Veesy.Presentation.Model.Admin;

public class SubscriptionOverviewViewModel
{
    public decimal EarningsThisMonth { get; set; } 
    public decimal EarningsThisYear { get; set; }
    public int NumberPayingUsers { get; set; }
    public List<AdminHelper.EarningYearGroupedByMonthDto> EarningGraph { get; set; }
    public List<SubscriptionPlan> SubscriptionPlans { get; set; }
    public List<MyUserSubscriptionPlan?> ActiveSubscription { get; set; }
}