using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories;

public interface IAnalyticRepository : IRepositoryBase<ReferralLink>
{
    public ReferralLinkTracking CreateReferralLinkTracking(ReferralLinkTracking referralLinkTracking);
    
}