using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories.Impl;

public class TrackingFormRepository: RepositoryBase<TrackingForm>, ITrackingFormRepository 
{
    public TrackingFormRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }
}