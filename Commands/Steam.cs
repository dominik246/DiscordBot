using Discord.Commands;

using DiscordBot.Services;

using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Steam : ModuleBase<SocketCommandContext>
    {
        [Command("steam")]
        [Summary("Returns a steam game.")]
        public async Task SteamAsync([Summary("Game to search.")][Remainder] string game)
        {
            var result = new SteamService().GetInfo(game);
            string x = "";

            try
            {
                x = result[0];
            }
            catch
            {
                x = "Game not found. Weird.";
            }
            finally
            {
                await ReplyAsync(x);
            }

        }
    }
}
