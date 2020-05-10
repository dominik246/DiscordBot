using Discord.WebSocket;

using DiscordBot.Commands;
using DiscordBot.DiscordBot.Handlers;
using DiscordBot.DiscordBot.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class SteamService : ISteamService
    {
        private readonly IGoogleApiHelper _api;
        private readonly IJsonHelper _handler;
        private readonly IEmbedHandler _embed;

        public SteamService(IGoogleApiHelper api, IJsonHelper handler, IEmbedHandler embed)
        {
            _api = api;
            _handler = handler;
            _embed = embed;
        }

        public async Task<(ulong, ulong, string)> GetAnswerAsync(CommandHandlingService commandService, string game)
        {
            ulong newAuthor = commandService.Message.Author.Id + 1; // Needs to make sure authors don't match at the first iteration
            SocketUser newAuthorAsSocketUser = commandService.Message.Author;

            ulong originalAuthor = 0;
            string originalMessage = "";
            string newMessage = "";
            int index = 0;
            string reply = "";

            ulong firstDisposableMessage = 0;
            ulong secondDisposableMessage = 0;

            // Searches for possible matches on steam and returns them as a json string
            string searchResult = await _api.GetResult(game);
            List<(string, string)> jsonResult = await _handler.Parse(searchResult);

            while (true)
            {

                // Sets newAuthor to the author of the newest message
                if (originalAuthor != 0 && !originalMessage.Equals(newMessage))
                {
                    newAuthor = commandService.Message.Author.Id;
                    newMessage = commandService.Message.Content;
                }
                // Only executes once so that it sets the user who originally initialized the command
                if (!commandService.Message.Author.IsBot && originalAuthor == 0 && string.IsNullOrEmpty(originalMessage))
                {
                    originalAuthor = commandService.Message.Author.Id;
                    originalMessage = commandService.Message.Content;
                }

                // Builds the embed and sends it to the channel
                if (index.Equals(0))
                {
                    reply = await _embed.Build(commandService, jsonResult);
                    firstDisposableMessage = commandService.Message.Id;
                    index = 1;
                }

                if (reply == "Game not found. Weird.")
                {
                    break;
                }

                // Will only execute if we got a reply from the same user
                if (originalAuthor != 0 && originalAuthor.Equals(newAuthor) && !newAuthorAsSocketUser.IsBot && !originalMessage.Equals(newMessage))
                {
                    // Checks if it's a number and if the number is in the list
                    if (int.TryParse(commandService.Message.Content, out int answer))
                    {
                        secondDisposableMessage = commandService.Message.Id;
                        if (answer < jsonResult.Count)
                        {
                            reply = jsonResult[answer - 1].Item2;
                            break;
                        }
                        else
                        {
                            reply = "That number is not in the list.";
                            break;
                        }
                    }
                    else
                    {
                        reply = "Not a number.";
                        break;
                    }

                }
            }
            return (firstDisposableMessage, secondDisposableMessage, reply);
        }
    }
}
