using Microsoft.EntityFrameworkCore;
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
        var portfolio = _uoW.PortfolioRepository.FindByCondition(w => w.MyUserId == userId && w.Id == portfolioId).SingleOrDefault();
        if (portfolio == null)
            throw new Exception($"Portfolio not found with id {portfolioId}.");
        return portfolio;
    }

    public async Task<Portfolio> AddPortfolio(Portfolio portfolio, MyUser user)
    {
        var result = await _uoW.PortfolioRepository.Create(portfolio);
        await _uoW.CommitAsync(user);
        return result;
    }

    public async Task<Portfolio> UpdatePortfolio(Portfolio portfolio, MyUser user)
    {
        var result = _uoW.PortfolioRepository.Update(portfolio);
        await _uoW.CommitAsync(user);
        return result;
    }

    public IEnumerable<Portfolio> GetPortfoliosByUser(MyUser user)
    {
        return _uoW.PortfolioRepository.FindByCondition(s => s.MyUserId == user.Id);
    }

    public IEnumerable<Portfolio> GetPortfoliosByUserWithMedia(MyUser user)
    {
        return _uoW.PortfolioRepository.FindByCondition(s => s.MyUserId == user.Id)
            .Include(s => s.PortfolioMedias)
            .ThenInclude(s => s.Media);

    }
}