using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public interface IDmOwnerHelper
    {
        Task SendDm(DiscordSocketClient client, LogMessage message, Embed embed);
    }
}