using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories;

public interface IUsedSoftwareRepository: IRepositoryBase<UsedSoftware>
{
    public List<MyUserUsedSoftware> GetUsedSoftwaresByUser(MyUser user);
    public void DeleteMyUserUsedSoftwares(List<MyUserUsedSoftware> entities);
    Task AddMyUserUsedSoftwares(List<MyUserUsedSoftware> entities);
}