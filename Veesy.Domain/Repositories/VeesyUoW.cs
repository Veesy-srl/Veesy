using Veesy.Domain.Data;
using Veesy.Domain.Models;
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
    
    public void Commit(MyUser user) => this._dbContext.SaveChanges();
    public async Task CommitAsync(MyUser user)
    {
        await _dbContext.SaveChangesAsync(user.Id);
    }
    public async Task CommitAsync(string userId)
    {
        await _dbContext.SaveChangesAsync(userId);
    }
    public void Rollback() => _dbContext.Dispose();
    public async Task RollbackAsync() => await _dbContext.DisposeAsync();
    
    public ApplicationDbContext DbContext { get; }

    private IMyUserRepository _myUserRepository;
    public IMyUserRepository MyUserRepository => _myUserRepository ??= new MyUserRepository(_dbContext);

    private IMediaRepository _mediaRepository;
    public IMediaRepository MediaRepository => _mediaRepository ??= new MediaRepository(_dbContext);
    private IUsedSoftwareRepository _usedSoftwareRepository;
    
    public IPortfolioRepository PortfolioRepository => _portfolioRepository ??= new PortfolioRepository(_dbContext);
    private IPortfolioRepository _portfolioRepository;
    
    public IUsedSoftwareRepository UsedSoftwareRepository => _usedSoftwareRepository ??= new UsedSoftwareRepository(_dbContext);
    
    private ISkillRepository _skillRepository;
    
    public ISkillRepository SkillRepository => _skillRepository ??= new SkillRepository(_dbContext);
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}