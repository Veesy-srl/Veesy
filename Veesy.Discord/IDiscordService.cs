using Veesy.Domain.Exceptions;
using Veesy.Service.Dtos;

namespace Veesy.Discord;

public interface IDiscordService
{
    public Task<DiscordUserDto> GetDiscordUser(string code);
}