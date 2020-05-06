using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Services;

using System.Collections.Generic;
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
            EmbedBuilder eb = new EmbedBuilder();
            SteamService ss = new SteamService();

            ulong newAuthor = _commandService.Message.Author.Id + 1; // Needs to make sure authors don't match at the first iteration
            SocketUser newAuthorAsSocketUser = _commandService.Message.Author;

            string reply = "";
            int index = 0;
            ulong originalAuthor = 0;
            string originalMessage = "";
            string newMessage = "";
            ulong firstDisposableMessage = 0;
            ulong secondDisposableMessage = 0;

            // Searches for possible matches on steam and returns a list of them
            List<(string, string)> result = await ss.GetInfo(game);

            while (true)
            {
                // Sets newAuthor to the author of the newest message
                if (originalAuthor != 0 && !originalMessage.Equals(newMessage))
                {
                    newAuthor = _commandService.Message.Author.Id;
                    newMessage = _commandService.Message.Content;
                }
                // Only executes once so that it sets the user who originally initialized the command
                if (!_commandService.Message.Author.IsBot && originalAuthor == 0 && string.IsNullOrEmpty(originalMessage))
                {
                    originalAuthor = _commandService.Message.Author.Id;
                    originalMessage = _commandService.Message.Content;
                }

                // Makes sure it only runs once in the lifetime of the command
                if (index.Equals(0))
                {
                    // Checks if we got any result back
                    if (result.Count > 0)
                    {
                        //await ReplyAsync("Which one?");
                        await Task.Run(async () =>
                        {
                            eb.WithTitle("Result of steam search:");
                            foreach ((string, string) s in result)
                            {
                                eb.AddField($"{++index}) ", s.Item1);
                            }
                            eb.WithColor(Color.Green);
                            await ReplyAsync(message: "Which one?", embed: eb.Build());
                            firstDisposableMessage = _commandService.Message.Id;
                        });
                    }
                    else
                    {
                        reply = "Game not found. Weird.";
                        break;
                    }
                }

                // Will only execute if we got a reply from the same user
                if (originalAuthor != 0 && originalAuthor.Equals(newAuthor) && !newAuthorAsSocketUser.IsBot && !originalMessage.Equals(newMessage))
                {
                    // Checks if it's a number and if the number is in the list
                    if (int.TryParse(_commandService.Message.Content, out int answer))
                    {
                        secondDisposableMessage = _commandService.Message.Id;
                        if (answer < result.Count)
                        {
                            reply = result[answer - 1].Item2;
                            break;
                        }
                        else
                        {
                            reply = "That number is not in the list.";
                        }
                    }
                    else
                    {
                        reply = "Not a number.";
                        break;
                    }

                }
            }
            // Cleanup after task is done
            await Context.Channel.DeleteMessageAsync(firstDisposableMessage);
            await Context.Channel.DeleteMessageAsync(secondDisposableMessage);

            // Finally replys the link of the query
            await ReplyAsync(reply);
        }
    }
}
