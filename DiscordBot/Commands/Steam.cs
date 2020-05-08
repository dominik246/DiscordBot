using Discord.Commands;

using DiscordBot.Services;

using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Steam : ModuleBase<SocketCommandContext>
    {
        private readonly CommandHandlingService _commandService;

        public Steam(CommandHandlingService commandService)
        {
            _commandService = commandService;
        }

        [Command("steam", RunMode = RunMode.Async)]
        [Name("steam <game to search>")]
        [Summary("Returns a steam game.")]
        public async Task SteamAsync([Summary("Game to search.")][Remainder] string game)
        {
            SteamService ss = new SteamService();
            (ulong, ulong, string) result = await ss.GetAnswerAsync(_commandService, game);

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
