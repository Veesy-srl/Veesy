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
}