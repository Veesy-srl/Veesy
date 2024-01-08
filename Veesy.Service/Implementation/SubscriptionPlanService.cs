using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories;
using Veesy.Service.Interfaces;

namespace Veesy.Service.Implementation;

public class SubscriptionPlanService : ISubscriptionPlanService
{
    private readonly IVeesyUoW _uoW;

    public SubscriptionPlanService(IVeesyUoW uoW)
    {
        _uoW = uoW;
    }

    public SubscriptionPlan GetSubscriptionByUserId(string userId)
    {
        return _uoW.DbContext.MyUserSubscriptionPlans
            .Include(s => s.SubscriptionPlan)
            .Where(s => s.MyUserId == userId)
            .OrderBy(s => s.CreateRecordDate)
            .LastOrDefault().SubscriptionPlan;
    }

    public decimal GetEarningsByMonth(int month)
    {
        return _uoW.DbContext.MyUserSubscriptionPlans
            .Include(s => s.SubscriptionPlan)
            .Where(s => s.SubscriptionPlan.Price >= 0)
            .Sum(s => s.SubscriptionPlan.Price);
    }

    public List<IGrouping<string,MyUserSubscriptionPlan>> GetMyUserSubscriptionPlanGoupByUserId()
    {
        return _uoW.DbContext.MyUserSubscriptionPlans
            .Include(s => s.SubscriptionPlan)
            .OrderBy(s => s.CreateRecordDate)
            .GroupBy(s => s.MyUserId).ToList();
    }
}