using Discord.Commands;

using DiscordBot.DiscordBot.Services;

using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Spam : ModuleBase<SocketCommandContext>
    {
        private readonly CommandHandlingService _commandService;
        public Spam(CommandHandlingService commandService)
        {
            _commandService = commandService;
        }

        [Command("spam", RunMode = RunMode.Async)]
        [Name("spam [optional]<amount>")]
        [Summary("Spams random stuff in the given channel.")]
        [RequireBotPermission(Discord.GuildPermission.ManageMessages)]
        [RequireUserPermission(Discord.GuildPermission.ManageMessages)]
        public async Task SpamAsync(uint count = 5)
        {
            SpamService ss = new SpamService();
            await ss.SpamString(_commandService, count);
            await ReplyAsync("``Finished!``");
        }


    }
}
