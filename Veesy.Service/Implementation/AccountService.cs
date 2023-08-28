using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Exception;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories;
using Veesy.Service.Interfaces;

namespace Veesy.Service.Implementation;

public class AccountService : IAccountService
{
    private readonly IVeesyUoW _uoW;

    public AccountService(IVeesyUoW uoW)
    {
        _uoW = uoW;
    }

    public async Task<ResultDto> UpdateUserProfile(MyUser user)
    {
        _uoW.MyUserRepository.Update(user);
        await _uoW.CommitAsync();
        return new ResultDto(true, "");
    }

    public List<UsedSoftware> GetUsedSoftware()
    {
        return _uoW.UsedSoftwareRepository.FindAll().ToList();
    }

    public List<UsedSoftware> GetUsedSoftwareWithUser(MyUser user)
    {
        return _uoW.UsedSoftwareRepository.FindAll().Include(s => s.MyUserUsedSoftwares.Where(s => s.MyUserId == user.Id)).ToList();
    }

    public List<MyUserUsedSoftware> GetUsedSoftwaresByUser(MyUser user)
    {
        return _uoW.UsedSoftwareRepository.GetUsedSoftwaresByUser(user);
    }

    public async Task<ResultDto> UpdateMyUserUsedSoftware(List<MyUserUsedSoftware> usedSoftwareToDelete, List<MyUserUsedSoftware> usedSoftwareToAdd)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.UsedSoftwareRepository.DeleteMyUserUsedSoftwares(usedSoftwareToDelete);
                await _uoW.UsedSoftwareRepository.AddMyUserUsedSoftwares(usedSoftwareToAdd);
                await _uoW.CommitAsync();
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
}