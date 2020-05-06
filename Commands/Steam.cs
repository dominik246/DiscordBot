using Discord.Commands;

using DiscordBot.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Steam : ModuleBase<SocketCommandContext>
    {
        [Command("steam")]
        [Name("steam <game to search>")]
        [Summary("Returns a steam game.")]
        public async Task SteamAsync([Summary("Game to search.")][Remainder] string game)
        {
            SteamService ss = new SteamService();
            List<string> result = await ss.GetInfo(game);

            string reply = "";

            try
            {
                reply = result[0];
            }
            catch
            {
                reply = "Game not found. Weird.";
            }
            finally
            {
                await ReplyAsync(reply);
            }

        }
    }
}
