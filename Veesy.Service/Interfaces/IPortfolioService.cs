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
    List<PortfolioMedia> GetPortfliosMediaByPortfolioIdToReorder(Guid portfolioId, int index);
    IEnumerable<PortfolioMedia> GetPortfliosMediaByPortfoliosId(List<Guid> portfoliosId);
    Task<ResultDto> UpdatePortfolioMedias(List<PortfolioMedia> portfoliosMediaToDelete, List<PortfolioMedia> portfoliosMediaToAdd, List<PortfolioMedia> portfoliosMediaToUpdate, MyUser userInfo);
    Portfolio? GetMainPortfolioByUser(MyUser user);
    Task UpdatePortfolios(List<Portfolio> portfoliosToUpdate, MyUser user);
    Task DeletePortfolio(Portfolio portfolio, MyUser userInfo);
    Task DeletePortfolioAndChangeMain(Portfolio portfolio, MyUser userInfo);
    Portfolio GetPortfolioByIdWithPortfoliosMedia(Guid portfolioId, string userId);
    IEnumerable<Portfolio> GetPortfoliosByMedia(Guid imgToDelete);
    IEnumerable<Portfolio> GetPortfoliosByMedias(List<Guid> imgToDelete);
    Portfolio? GetPortfolioByIdForPreview(Guid id);
}