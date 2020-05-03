using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Hello : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Returns the given input.")]
        public async Task HelloAsync([Summary("The given input")] string message)
        {
            await ReplyAsync(message);
        }
    }
}
