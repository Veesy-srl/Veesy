using Veesy.Domain.Data;

namespace Veesy.Domain.Repositories;

public interface IVeesyUoW
{
    
    void Commit();
    void Rollback();
    Task CommitAsync();
    Task RollbackAsync();
    public ApplicationDbContext DbContext { get; }
    IMyUserRepository MyUserRepository { get; }
    IUsedSoftwareRepository UsedSoftwareRepository { get; }
}