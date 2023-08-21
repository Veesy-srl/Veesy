using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories.Impl;

public class UsedSoftwareRepository : RepositoryBase<UsedSoftware>, IUsedSoftwareRepository
{
    public UsedSoftwareRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public List<MyUserUsedSoftware> GetUsedSoftwaresByUser(MyUser user)
    {
        return _applicationDbContext.MyUserUsedSoftwares
            .Include(s => s.UsedSoftware)
            .Where(s => s.MyUserId == user.Id)
            .ToList();
    }

    public void DeleteMyUserUsedSoftwares(List<MyUserUsedSoftware> entities)
    {
        _applicationDbContext.MyUserUsedSoftwares.RemoveRange(entities);
    }

    public async Task AddMyUserUsedSoftwares(List<MyUserUsedSoftware> entities)
    {
        await _applicationDbContext.MyUserUsedSoftwares.AddRangeAsync(entities);
    }
}