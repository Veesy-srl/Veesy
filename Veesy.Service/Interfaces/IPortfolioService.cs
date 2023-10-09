using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;

namespace Veesy.Service.Interfaces;

public interface IPortfolioService
{
    Portfolio GetPortfolioById(Guid portfolioId, string userId);
    Task<Portfolio> AddPortfolio(Portfolio portfolio, MyUser user);
    Task<Portfolio> UpdatePortfolio(Portfolio portfolio, MyUser user);
    IEnumerable<Portfolio> GetPortfoliosByUser(MyUser user);
    IEnumerable<Portfolio> GetPortfoliosByUserWithMedia(MyUser user);
    IEnumerable<PortfolioMedia> GetPortfliosMediaByMediaId(Guid mediaId);
    Task<ResultDto> UpdatePortfolioMedias(List<PortfolioMedia> portfoliosMediaToDelete, List<PortfolioMedia> portfoliosMediaToAdd, MyUser userInfo);
}