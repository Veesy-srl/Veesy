using Veesy.Domain.Models;
using Veesy.Domain.Repositories;
using Veesy.Service.Interfaces;

namespace Veesy.Service.Implementation;

public class PortfolioService : IPortfolioService
{
    private readonly IVeesyUoW _uoW;

    public PortfolioService(IVeesyUoW uoW)
    {
        _uoW = uoW;
    }

    public Portfolio GetPortfolioById(Guid portfolioId, string userId)
    {
        return _uoW.PortfolioRepository.GetPortfolioById(portfolioId, userId);
    }
}