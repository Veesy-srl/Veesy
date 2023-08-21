using Veesy.Domain.Exception;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories;

namespace Veesy.Service.Interfaces;

public interface IAccountService
{
    Task<ResultDto> UpdateUserProfile(MyUser user);
}