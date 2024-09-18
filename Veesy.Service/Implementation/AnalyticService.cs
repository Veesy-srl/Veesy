using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories;
using Veesy.Service.Interfaces;

namespace Veesy.Service.Implementation;

public class AnalyticService : IAnalyticService
{
    private readonly IVeesyUoW _uoW;

    public AnalyticService(IVeesyUoW uoW)
    {
        _uoW = uoW;
    }

    public ReferralLink? GetReferralByStartEndpoint(string startEndpoint)
    {
        return _uoW.AnalyticRepository.FindByCondition(s => s.Endpoint == startEndpoint).SingleOrDefault();
    }

    public ReferralLink? GetReferralById(Guid id)
    {
        return _uoW.AnalyticRepository.FindByCondition(s => s.Id == id)
            .Include(s => s.ReferralLinkTrackings)
            .SingleOrDefault();
    }

    public async Task SaveReferralTrackingLink(ReferralLinkTracking referralLinkTracking)
    {
        _uoW.DbContext.ReferralLinkTrackings.Add(referralLinkTracking);
        await _uoW.CommitAsync(new MyUser());
    }

    public List<ReferralLink> GetReferralLinks()
    {
        return _uoW.AnalyticRepository.FindAll().ToList();
    }

    public List<ReferralLinkTracking> GetReferralLinkTrackings()
    {
        return _uoW.DbContext.ReferralLinkTrackings
            .Where(s => s.ReferralLinkId != null)
            .Include(s => s.ReferralLink)
            .OrderByDescending(s => s.LastAccess)
            .ToList();
    }

    public async Task UpdateReferralLink(ReferralLink referralLink)
    {
        _uoW.AnalyticRepository.Update(referralLink);
        await _uoW.CommitAsync(new MyUser());
    }

    public async Task AddReferralLink(ReferralLink referralLink)
    {
        await _uoW.AnalyticRepository.Create(referralLink);
        await _uoW.CommitAsync(new MyUser());
    }

    public async Task RemoveReferralLink(ReferralLink? referralLink)
    {
        _uoW.AnalyticRepository.Delete(referralLink);
        await _uoW.CommitAsync(new MyUser());
    }

    public async Task AddForm(TrackingForm trackingForm, MyUser user)
    {
        await _uoW.TrackingFormRepository.Create(trackingForm);
        await _uoW.CommitAsync(user);
    }

    public List<TrackingForm> GetCreatorsForms()
    {
        return _uoW.TrackingFormRepository.FindByCondition(x => x.FormType == VeesyConstants.FormType.CreatorType)
            .ToList();
    }
}