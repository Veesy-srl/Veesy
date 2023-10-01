using Veesy.Domain.Models;

namespace Veesy.Service.Interfaces;

public interface IPortfolioService
{
    Portfolio GetPortfolioById(Guid portfolioId, string userId);
    Task<Portfolio> AddPortfolio(Portfolio portfolio, MyUser user);
    Task<Portfolio> UpdatePortfolio(Portfolio portfolio, MyUser user);
}