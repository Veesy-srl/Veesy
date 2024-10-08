using System.Threading.Tasks;
using Veesy.Domain.Data;
using Veesy.Domain.Models;

namespace Veesy.Domain.Repositories;

public interface IVeesyUoW
{
    
    void Commit(MyUser user);
    void Rollback();
    Task CommitAsync(string userId);
    Task CommitAsync(MyUser user);
    Task RollbackAsync();
    public ApplicationDbContext DbContext { get; }
    IMyUserRepository MyUserRepository { get; }
    IUsedSoftwareRepository UsedSoftwareRepository { get; }
    ISkillRepository SkillRepository { get; }
    IMediaRepository MediaRepository { get; }
    IPortfolioRepository PortfolioRepository { get; }
    IAnalyticRepository AnalyticRepository { get; }
    ITrackingFormRepository TrackingFormRepository { get; }
}