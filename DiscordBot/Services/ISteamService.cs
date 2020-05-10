using DiscordBot.Commands;

using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public interface ISteamService
    {
        Task<(ulong, ulong, string)> GetAnswerAsync(CommandHandlingService commandService, string game);
    }
}