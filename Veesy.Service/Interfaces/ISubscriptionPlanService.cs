using Veesy.Domain.Models;

namespace Veesy.Service.Interfaces;

public interface ISubscriptionPlanService
{
    SubscriptionPlan GetSubscriptionByUserId(string userId);
}