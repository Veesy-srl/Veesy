using Veesy.Domain.Models;

namespace Veesy.Service.Interfaces;

public interface IAnalyticService
{
    ReferralLink? GetReferralByStartEndpoint(string startEndpoint);
    ReferralLink? GetReferralById(Guid id);
    Task SaveReferralTrackingLink(ReferralLinkTracking referralLinkTracking);
    List<ReferralLink> GetReferralLinks();
    List<ReferralLinkTracking> GetReferralLinkTrackings();
    Task UpdateReferralLink(ReferralLink referralLink);
    Task AddReferralLink(ReferralLink referralLink);
    Task RemoveReferralLink(ReferralLink? referralLink);
}