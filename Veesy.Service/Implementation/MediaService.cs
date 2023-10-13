using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Data;
using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories;
using Veesy.Domain.Repositories.Impl;
using Veesy.Service.Interfaces;

namespace Veesy.Service.Implementation;

public class MediaService : IMediaService
{
    private readonly IVeesyUoW _uoW;
    private readonly UserManager<MyUser> _userManager;

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

    public async Task DeleteMedia(Media media, MyUser user)
    {
        _uoW.MediaRepository.Delete(media);
        await _uoW.CommitAsync(user);
    }

    public List<(string, string, string)> GetRandomMediaWithUsername(int count)
    {
        var UsersWithImage = _uoW.MyUserRepository.GetOnlyUserWithImage();
        Random random = new Random();

        if (UsersWithImage.Count > 0)
        {
            List<(string, string, string)> result = new List<(string, string, string)>();

            List<string> allImages = UsersWithImage
                .SelectMany(user => user.Medias.Select(media => media.FileName))
                .ToList();

            while (result.Count < count && allImages.Count > 0)
            {
                int randomImageIndex = random.Next(allImages.Count);
                string userImage = allImages[randomImageIndex];
                allImages.RemoveAt(randomImageIndex);

                var user = UsersWithImage.FirstOrDefault(u => u.Medias.Any(m => m.FileName == userImage));
                if (user != null)
                {
                    string randomUser = user.UserName;
                    string userProfileImage = user.ProfileImageFileName;
                    result.Add((userImage, userProfileImage, randomUser));
                }
            }

            return result;
        }

        return new List<(string, string, string)> { (null, null, null) };
    }

    public List<(string FileName, long Size)> GetMediasNameAndSizeByUserId(string userId)
    {
        return _uoW.MediaRepository.FindByCondition(s => s.MyUserId == userId).Select(s => new ValueTuple<string, long>(s.OriginalFileName, s.Size)).ToList();
    }
}