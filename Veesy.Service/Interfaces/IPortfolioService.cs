using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;

namespace Veesy.Service.Interfaces;

public interface IPortfolioService
{
    Portfolio GetPortfolioById(Guid portfolioId, string userId);
    Portfolio GetPortfolioByIdToUpdate(Guid portfolioId, string userId);
    Task<Portfolio> AddPortfolio(Portfolio portfolio, MyUser user);
    Task<Portfolio> UpdatePortfolio(Portfolio portfolio, MyUser user);
    IEnumerable<Portfolio> GetPortfoliosByUser(MyUser user);
    IEnumerable<Portfolio> GetPortfoliosByUserWithMedia(MyUser user);
    IEnumerable<PortfolioMedia> GetPortfoliosMediaByMediaId(Guid mediaId);
    List<PortfolioMedia> GetPortfoliosMediaByPortfolioIdToReorder(Guid portfolioId, int index);
    IEnumerable<PortfolioMedia> GetPortfoliosMediaByPortfoliosId(List<Guid> portfoliosId);
    Task<ResultDto> UpdatePortfolioMedias(List<PortfolioMedia> portfoliosMediaToDelete, List<PortfolioMedia> portfoliosMediaToAdd, List<PortfolioMedia> portfoliosMediaToUpdate, MyUser userInfo);
    Portfolio? GetMainPortfolioByUser(MyUser user);
    (string, Guid) GetMainPortfolioNameByUserId(string userId);
    Portfolio? GetMainPortfolioByUserWithMedias(MyUser user);
    Task UpdatePortfolios(List<Portfolio> portfoliosToUpdate, MyUser user);
    Task DeletePortfolio(Portfolio portfolio, MyUser userInfo);
    Task DeletePortfolioAndChangeMain(Portfolio portfolio, MyUser userInfo);
    Portfolio GetPortfolioByIdWithPortfoliosMedia(Guid portfolioId, string userId);
    Portfolio GetPortfolioByIdWithPortfoliosMedia(Guid portfolioId);
    IEnumerable<Portfolio> GetPortfoliosByMedia(Guid imgToDelete);
    IEnumerable<Portfolio> GetPortfoliosByMedias(List<Guid> imgToDelete);
    Portfolio? GetPortfolioByIdForPreview(Guid id);
    int GetPortfoliosNumberByUser(string userId);
    int GetPublicPortfoliosNumberByUser(string userId);
    Task SetPortfoliosToDraftByIds(List<Guid> portfolioDtoPortfolioSelected, MyUser user);
    List<string> GetAllPortfolioNameDifferentByOne(Guid id, MyUser user);
    Portfolio? GetPortfolioByUserAndName(string user, string portfolioname);
}