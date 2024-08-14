using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories.Impl;

public class AnalyticRepository : RepositoryBase<ReferralLink>, IAnalyticRepository
{
    public AnalyticRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public ReferralLinkTracking CreateReferralLinkTracking(ReferralLinkTracking referralLinkTracking)
    {
        throw new NotImplementedException();
    }
}