using Discord.Commands;
using DiscordBot.Services;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Steam : ModuleBase<SocketCommandContext>
    {
        private readonly ISteamService _service;

        public Steam(ISteamService service)
        {
            _service = service;
        }

        [Command("steam", RunMode = RunMode.Async)]
        [Name("steam <game to search>")]
        [Summary("Returns a steam game.")]
        public async Task SteamAsync([Summary("Game to search.")][Remainder] string game)
        {
            (ulong, ulong, string) result = await _service.GetAnswerAsync(game);

            if (!result.Item3.Equals("Game not found. Weird."))
            {
                // Cleanup after task is done
                await Context.Channel.DeleteMessageAsync(result.Item1);
                await Context.Channel.DeleteMessageAsync(result.Item2);
            }

            // Finally replys the link of the query
            await ReplyAsync(result.Item3);
        }
    }
}
