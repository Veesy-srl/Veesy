using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories;
using Veesy.Service.Interfaces;

namespace Veesy.Service.Implementation;

public class MediaService : IMediaService
{
    private readonly IVeesyUoW _uoW;
    private readonly UserManager<MyUser> _userManager;

    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public MediaService(UserManager<MyUser> userManager, IVeesyUoW uoW)
    {
        _uoW = uoW;
        _userManager = userManager;
    }

    public List<Media> GetAllByUserId(MyUser user)
    {
        return _uoW.MediaRepository.FindByCondition(s => s.MyUserId == user.Id).OrderBy(s => s.CreateRecordDate).ToList();
    }

    public async Task<Media> AddMedia(Media media, MyUser user)
    {
        var res = await _uoW.MediaRepository.Create(media);
        await _uoW.CommitAsync(user);
        return res;
    }

    public Media? GetMediaById(Guid id)
    {
        return _uoW.MediaRepository.FindByCondition(s => s.Id == id).SingleOrDefault();
    }
    public Media? GetPreviousMediaByDate(DateTime mediaSelectedCreateRecordDate, MyUser user)
    {
        return _uoW.MediaRepository
            .FindByCondition(s => s.MyUserId == user.Id && s.CreateRecordDate < mediaSelectedCreateRecordDate)
            .OrderByDescending(s => s.CreateRecordDate)
            .FirstOrDefault();
    }

    public Media? GetNextMediaByDate(DateTime mediaSelectedCreateRecordDate, MyUser user)
    {
        return _uoW.MediaRepository
            .FindByCondition(s => s.MyUserId == user.Id && s.CreateRecordDate > mediaSelectedCreateRecordDate)
            .OrderBy(s => s.CreateRecordDate)
            .FirstOrDefault();
    }

    public async Task<ResultDto> UpdateMedia(Media media, MyUser user)
    {
        _uoW.MediaRepository.Update(media);
        await _uoW.CommitAsync(user);
        return new ResultDto(true, "");
    }

    public long GetSizeMediaStorageByUserId(string userId)
    {
        return _uoW.MediaRepository.FindByCondition(s => s.MyUserId == userId).Sum(s => s.Size);
    }

    public async Task<ResultDto> DeleteMediasAndUpdatePortfolios(List<Media> medias, List<Portfolio> portfolios, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.MediaRepository.DeleteRange(medias);
                _uoW.PortfolioRepository.UpdateRange(portfolios);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultDto(false, ex.Message);
            }
        }
    }
    
    public async Task<ResultDto> DeleteMediaAndUpdatePortfolios(Media media, List<Portfolio> portfolios, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.MediaRepository.Delete(media);
                _uoW.PortfolioRepository.UpdateRange(portfolios);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultDto(false, ex.Message);
            }
        }
    }

    public List<MyUser> GetRandomMediaWithUsername(int count)
    {
        return _uoW.MyUserRepository.GetOnlyRandomUserWithImage(count);
    }

    public List<(string FileName, long Size)> GetMediasNameAndSizeByUserId(string userId)
    {
        return _uoW.MediaRepository.FindByCondition(s => s.MyUserId == userId).Select(s => new ValueTuple<string, long>(s.OriginalFileName, s.Size)).ToList();
    }

    public Media GetMediaByIdWithPortfoliosMedia(Guid imgCode)
    {
        return _uoW.MediaRepository.FindByCondition(s => s.Id == imgCode).Include(s => s.PortfolioMedias).SingleOrDefault();
    }

    public List<string> GetAllMediaNameByUser(MyUser userInfo)
    {
        return _uoW.MediaRepository.FindByCondition(s => s.MyUserId == userInfo.Id).Select(s => s.OriginalFileName).ToList();
    }

    public List<Media> GetMediasByIdWithPortfoliosMedia(List<Guid> imgToDelete)
    {
        return _uoW.MediaRepository.FindByCondition(s => imgToDelete.Contains(s.Id)).Include(s => s.PortfolioMedias).ToList();
    }
}