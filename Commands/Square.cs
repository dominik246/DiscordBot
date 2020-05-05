using Discord.Commands;

using System;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Square : ModuleBase<SocketCommandContext>
    {
        [Command("square")]
        [Summary("Squares a given number.")]
        public async Task SquareAsync([Summary("The number to square")] string num)
        {
            if (int.TryParse(num, out int result))
            {
                await ReplyAsync($"{result}^2 = {Math.Pow(result, 2)}");
            }
            else
            {
                await ReplyAsync("Not a number.");
            }
        }
    }
}
