using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Square : ModuleBase<SocketCommandContext>
    {
        [Command("square")]
        [Summary("Squares a given number.")]
        public async Task SquareAsync([Summary("The number to square")] int num)
        {
            await ReplyAsync($"{num}^2 = {Math.Pow(num, 2)}");
        }
    }
}
