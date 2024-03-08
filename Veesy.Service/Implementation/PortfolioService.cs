using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Constants;
using Veesy.Domain.Exceptions;
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
        var portfolio = _uoW.PortfolioRepository
            .FindByCondition(w => w.MyUserId == userId && w.Id == portfolioId)
            .Include(s => s.PortfolioMedias.OrderBy(ob=>ob.SortOrder))
            .ThenInclude(s => s.Media)
            .SingleOrDefault();
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
            .ThenInclude(s => s.Media)
            .OrderByDescending(s => s.IsMain);
    }

    public IEnumerable<PortfolioMedia> GetPortfoliosMediaByMediaId(Guid mediaId)
    {
        return _uoW.DbContext.PortfolioMedias.Where(s => s.MediaId == mediaId);
    }

    public List<PortfolioMedia> GetPortfoliosMediaByPortfolioIdToReorder(Guid portfolioId, int index)
    {
        return _uoW.DbContext.PortfolioMedias.Where(s => s.PortfolioId == portfolioId && s.SortOrder > index).ToList();
    }

    public IEnumerable<PortfolioMedia> GetPortfoliosMediaByPortfoliosId(List<Guid> portfoliosId)
    {
        return _uoW.DbContext.PortfolioMedias.Where(s => portfoliosId.Contains(s.PortfolioId));
    }

    public async Task<ResultDto> UpdatePortfolioMedias(List<PortfolioMedia> portfoliosMediaToDelete, List<PortfolioMedia> portfoliosMediaToAdd, List<PortfolioMedia> portfoliosMediaToUpdate, MyUser userInfo)
    {
        await using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.DbContext.PortfolioMedias.RemoveRange(portfoliosMediaToDelete);
                _uoW.DbContext.PortfolioMedias.UpdateRange(portfoliosMediaToUpdate);
                _uoW.DbContext.PortfolioMedias.AddRange(portfoliosMediaToAdd);
                await _uoW.CommitAsync(userInfo);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public Portfolio? GetMainPortfolioByUser(MyUser user)
    {
        return _uoW.PortfolioRepository.FindByCondition(s => s.IsMain && s.MyUserId == user.Id).SingleOrDefault();
    }
    public Portfolio? GetMainPortfolioByUserWithMedias(MyUser user)
    {
        return _uoW.PortfolioRepository
            .FindByCondition(s => s.IsMain && s.MyUserId == user.Id)
            .Include(s => s.PortfolioMedias)
            .ThenInclude(s => s.Media)
            .SingleOrDefault();
    }

    public async Task UpdatePortfolios(List<Portfolio> portfoliosToUpdate, MyUser user)
    {
        _uoW.PortfolioRepository.UpdateRange(portfoliosToUpdate);
        await _uoW.CommitAsync(user);
    }

    public async Task DeletePortfolio(Portfolio portfolio, MyUser userInfo)
    {
        await using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.DbContext.PortfolioMedias.RemoveRange(portfolio.PortfolioMedias);
                _uoW.PortfolioRepository.Delete(portfolio);
                await _uoW.CommitAsync(userInfo);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public async Task DeletePortfolioAndChangeMain(Portfolio portfolio, MyUser userInfo)
    {
        await using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                var newMainPortfolio = _uoW.PortfolioRepository.FindByCondition(s => s.MyUserId == userInfo.Id)
                    .OrderByDescending(s => s.CreateRecordDate).FirstOrDefault();
                if (newMainPortfolio != null)
                    newMainPortfolio.IsMain = true;

                _uoW.PortfolioRepository.Delete(portfolio);
                await _uoW.CommitAsync(userInfo);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public Portfolio GetPortfolioByIdWithPortfoliosMedia(Guid portfolioId, string userId)
    {
        var portfolio = _uoW.PortfolioRepository.FindByCondition(w => w.MyUserId == userId && w.Id == portfolioId)
            .Include(s => s.PortfolioMedias)
            .SingleOrDefault();
        if (portfolio == null)
            throw new Exception($"Portfolio not found with id {portfolioId}.");
        return portfolio;
    }

    public Portfolio GetPortfolioByIdWithPortfoliosMedia(Guid portfolioId)
    {
        var portfolio = _uoW.PortfolioRepository.FindByCondition(w => w.Id == portfolioId)
            .Include(s => s.PortfolioMedias)
            .SingleOrDefault();
        if (portfolio == null)
            throw new Exception($"Portfolio not found with id {portfolioId}.");
        return portfolio;
    }

    public IEnumerable<Portfolio> GetPortfoliosByMedia(Guid imgToDelete)
    {
        return _uoW.PortfolioRepository.FindByCondition(s => s.PortfolioMedias.Select(s => s.MediaId).Contains(imgToDelete))
            .Include(s => s.PortfolioMedias);
    }

    public IEnumerable<Portfolio> GetPortfoliosByMedias(List<Guid> imgToDelete)
    {
        // recupero i portfoli che contengo le immagini selezionate
        var portfolios = _uoW.PortfolioRepository
            .FindByCondition(s => 
                s.PortfolioMedias
                    .Select(s=>s.MediaId)
                    .Any(w => imgToDelete.Contains(w))
            )
            .Include(s => s.PortfolioMedias);
        
        return portfolios;
    }
    
    public Portfolio? GetPortfolioByIdForPreview(Guid id)
    {
        return _uoW.PortfolioRepository.FindByCondition(s => s.Id == id)
            .Include(s => s.PortfolioMedias.OrderBy(s => s.SortOrder))
            .ThenInclude(s => s.Media)
            .Include(s => s.MyUser)
            .SingleOrDefault();
    }

    public int GetPortfoliosNumberByUser(MyUser user)
    {
        return _uoW.PortfolioRepository.FindByCondition(s => s.MyUserId == user.Id).ToList().Count;
    }

    public async Task SetPortfoliosToDraftByIds(List<Guid> portfolioDtoPortfolioSelected, MyUser user)
    {
        var portfolios = _uoW.PortfolioRepository.FindByCondition(s => portfolioDtoPortfolioSelected.Contains(s.Id)).ToList();
        portfolios.ForEach(s => s.Status = PortfolioContants.STATUS_DRAFT);
        _uoW.PortfolioRepository.UpdateRange(portfolios);
        await _uoW.CommitAsync(user);
    }

    public List<string> GetAllPortfolioNameDifferentByOne(Guid id, MyUser user)
    {
        return _uoW.PortfolioRepository.FindByCondition(s => s.Id != id && s.MyUserId == user.Id).Select(s => s.Name.ToUpper()).ToList();
    }
}