using Discord;
using DiscordBot.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public interface IEmbedHelper
    {
        Task<Embed> Build(CommandHandlingService commandService, List<(string, string)> content, string title = "");
    }
}