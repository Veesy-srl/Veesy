using Veesy.Domain.Data;
using Veesy.Domain.Repositories.Impl;

namespace Veesy.Domain.Repositories;

public class VeesyUoW : IVeesyUoW
{
    private readonly ApplicationDbContext _dbContext;

    public VeesyUoW(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        DbContext = dbContext;
    }
    
    public void Commit() => this._dbContext.SaveChanges();
    public async Task CommitAsync() => await _dbContext.SaveChangesAsync();
    public void Rollback() => _dbContext.Dispose();
    public async Task RollbackAsync() => await _dbContext.DisposeAsync();
    
    public ApplicationDbContext DbContext { get; }

    private IMyUserRepository _myUserRepository;
    public IMyUserRepository MyUserRepository => _myUserRepository ??= new MyUserRepository(_dbContext);
    
    private IUsedSoftwareRepository _usedSoftwareRepository;
    
    public IUsedSoftwareRepository UsedSoftwareRepository => _usedSoftwareRepository ??= new UsedSoftwareRepository(_dbContext);
    
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}