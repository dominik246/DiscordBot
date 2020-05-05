using Discord.Commands;

using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Say : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Returns the given input.")]
        public async Task SayAsync([Summary("The given input")][Remainder] string message)
        {
            await ReplyAsync(message);
        }
    }
}
