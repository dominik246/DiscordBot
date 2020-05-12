using DiscordBot.Commands;
using DiscordBot.DiscordBot.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class SpamService : ISpamService
    {
        private readonly IReadFromFileHelper _fileHandler;
        private readonly CommandHandlingService _commandService;

        public SpamService(CommandHandlingService commandService, IReadFromFileHelper fileHandler)
        {
            _commandService = commandService;
            _fileHandler = fileHandler;
        }

        public async Task SpamString(uint count)
        {
            await Task.Run(async () =>
            {
                foreach (string line in await _fileHandler.ReadAsync(count))
                {
                    string prefix = $"";

                    await _commandService.Message.Channel.SendMessageAsync(prefix + line);
                    Thread.Sleep(1000);
                }
            });
        }
    }
}
