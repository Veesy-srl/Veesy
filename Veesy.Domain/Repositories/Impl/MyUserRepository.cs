using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories.Impl;

public class MyUserRepository : RepositoryBase<MyUser>, IMyUserRepository 
{
    public MyUserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }
}