using DiscordBot.Commands;
using DiscordBot.DiscordBot.Handlers;

using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public interface ISpamService
    {
        Task SpamString(CommandHandlingService commandService, IReadFromFileHelper fileHandler, uint count = 5);
    }
}