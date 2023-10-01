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
        return _uoW.MyUserRepository.FindByCondition(s => s.Id == userId)
            .Include(s => s.SubscriptionPlan)
            .Select(s => s.SubscriptionPlan).FirstOrDefault();
    }
}