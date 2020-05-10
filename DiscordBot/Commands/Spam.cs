using Discord;
using Discord.Commands;

using DiscordBot.DiscordBot.Handlers;
using DiscordBot.DiscordBot.Services;

using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Spam : ModuleBase<SocketCommandContext>
    {
        //TODO: push DI to SpamService
        private readonly CommandHandlingService _commandService;
        private readonly SpamService _spam;
        private readonly IReadFromFileHelper _fileHandler;

        public Spam(CommandHandlingService commandService, SpamService spam, IReadFromFileHelper fileHandler)
        {
            _commandService = commandService;
            _spam = spam;
            _fileHandler = fileHandler;
        }

        [Command("spam", RunMode = RunMode.Async)]
        [Name("spam [optional]<amount>")]
        [Summary("Spams random stuff in the given channel.")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task SpamAsync(uint count = 5)
        {
            await _spam.SpamString(_commandService, _fileHandler, count);
            await ReplyAsync("``Finished!``");
        }


    }
}
