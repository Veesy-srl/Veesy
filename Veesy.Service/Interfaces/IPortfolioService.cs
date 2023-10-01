using Veesy.Domain.Models;

namespace Veesy.Service.Interfaces;

public interface IPortfolioService
{
    Portfolio GetPortfolioById(Guid portfolioId, string userId);
}