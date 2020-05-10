using DiscordBot.Commands;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public interface IEmbedHandler
    {
        Task<string> Build(CommandHandlingService commandService, List<(string, string)> content);
    }
}