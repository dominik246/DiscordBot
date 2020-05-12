using Discord;
using Discord.Commands;

using DiscordBot.DiscordBot.Services;

using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Spam : ModuleBase<SocketCommandContext>
    {
        private readonly ISpamService _spam;
        public Spam(ISpamService spam)
        {
            _spam = spam;
        }

        [Command("spam", RunMode = RunMode.Async)]
        [Name("spam [optional]<amount>")]
        [Summary("Spams random stuff in the given channel.")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task SpamAsync(uint count = 5)
        {
            await _spam.SpamString(count);
            await ReplyAsync("``Finished!``");
        }
    }
}
