using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Exceptions;
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
        var res = _uoW.DbContext.MyUserSubscriptionPlans
            .Where(s => s.CreateRecordDate.Month <= month)
            .Include(s => s.SubscriptionPlan)
            .GroupBy(s => s.MyUserId).ToList();

        var myUserSubscriptionPlans =
            res.Select(s => s.OrderByDescending(s => s.CreateRecordDate).FirstOrDefault()).ToList();

        return myUserSubscriptionPlans.Sum(s => s.SubscriptionPlan.Price);
    }

    public List<IGrouping<string,MyUserSubscriptionPlan>> GetMyUserSubscriptionPlanGoupByUserId()
    {
        return _uoW.DbContext.MyUserSubscriptionPlans
            .Include(s => s.SubscriptionPlan)
            .OrderBy(s => s.CreateRecordDate)
            .GroupBy(s => s.MyUserId).ToList();
    }

    public List<SubscriptionPlan> GetAllSubscriptionPlans()
    {
        return _uoW.DbContext.SubscriptionPlans.ToList();
    }

    public SubscriptionPlan GetSubscriptionPlanById(Guid id)
    {
        return _uoW.DbContext.SubscriptionPlans.FirstOrDefault(x => x.Id == id)!;
    }

    public async Task<ResultDto> EditSubscription(SubscriptionPlan subscription, MyUser user)
    {
        _uoW.DbContext.SubscriptionPlans.Update(subscription);
        await _uoW.CommitAsync(user);
        return new ResultDto(true, "");
    }

}