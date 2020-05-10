using Discord;

using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public interface ILoggerHelper
    {
        Task Log(LogMessage msg);
    }
}