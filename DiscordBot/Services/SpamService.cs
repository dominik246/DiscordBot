using DiscordBot.Commands;
using DiscordBot.DiscordBot.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class SpamService : ISpamService
    {
        public async Task SpamString(CommandHandlingService commandService, IReadFromFileHelper fileHandler, uint count)
        {
            await Task.Run(async () =>
            {
                foreach (string line in fileHandler.ReadAsync(count).Result)
                {
                    string prefix = $"";

                    await commandService.Message.Channel.SendMessageAsync(prefix + line);
                    Thread.Sleep(1000);
                }
            });
        }
    }
}
