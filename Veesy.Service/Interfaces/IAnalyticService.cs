using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public Task AddForm(TrackingForm trackingForm, MyUser user);
    public List<TrackingForm> GetCreatorsForms();
}