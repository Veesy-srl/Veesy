using Veesy.Domain.Exception;
using Veesy.Domain.Models;

namespace Veesy.Service.Interfaces;

public interface IAccountService
{
    Task<ResultDto> UpdateUserProfile(MyUser user);
    List<UsedSoftware> GetUsedSoftware();
    List<UsedSoftware> GetUsedSoftwareWithUser(MyUser user);
    List<MyUserUsedSoftware> GetUsedSoftwaresByUser(MyUser user);
    Task<ResultDto> UpdateMyUserUsedSoftware(List<MyUserUsedSoftware> usedSoftwareToDelete, List<MyUserUsedSoftware> usedSoftwareToAdd);
}