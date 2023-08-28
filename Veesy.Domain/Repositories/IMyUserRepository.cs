using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories;

public interface IMyUserRepository : IRepositoryBase<MyUser>
{
    List<CategoryWork> GetCategoriesWork();
}