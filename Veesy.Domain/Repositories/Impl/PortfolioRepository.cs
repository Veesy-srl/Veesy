using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;


namespace Veesy.Domain.Repositories.Impl;

public class PortfolioRepository : RepositoryBase<Portfolio>, IPortfolioRepository
{
    public PortfolioRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }
}