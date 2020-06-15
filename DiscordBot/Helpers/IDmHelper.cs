using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public interface IDmHelper
    {
        Task SendDm(DiscordSocketClient client, LogMessage message, Embed embed);
        Task SendDm(DiscordSocketClient client, ulong userId, Embed embed);
    }
}