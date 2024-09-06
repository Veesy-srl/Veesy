using System.Collections.Generic;
using System.Linq;
using Veesy.Domain.Models;

namespace Veesy.Service.Interfaces;

public interface ISubscriptionPlanService
{
    SubscriptionPlan GetSubscriptionByUserId(string userId);
    decimal GetEarningsByMonth(int month);
    List<IGrouping<string,MyUserSubscriptionPlan>> GetMyUserSubscriptionPlanGoupByUserId();
    public List<SubscriptionPlan> GetAllSubscriptionPlans();
}