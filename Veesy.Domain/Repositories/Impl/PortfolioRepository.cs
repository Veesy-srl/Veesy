using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;


namespace Veesy.Domain.Repositories.Impl;

public class PortfolioRepository : RepositoryBase<Portfolio>, IPortfolioRepository
{
    public PortfolioRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }


    public Portfolio GetPortfolioById(Guid portfolioId, string userId)
    {
        var portfolio = _applicationDbContext.Portfolios.SingleOrDefault(w => w.MyUserId == userId && w.Id == portfolioId);
        if (portfolio == null)
            throw new Exception($"Portfolio not found with id {portfolioId}.");
        return portfolio;
    }
}