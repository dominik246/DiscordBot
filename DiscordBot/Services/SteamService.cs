using Discord;
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
        private readonly IEmbedHelper _embed;
        private readonly CommandHandlingService _commandService;

        public SteamService(IGoogleApiHelper api, IJsonHelper handler, IEmbedHelper embed, CommandHandlingService commandService)
        {
            _api = api;
            _handler = handler;
            _embed = embed;
            _commandService = commandService;
        }

        public async Task<(ulong, ulong, string)> GetAnswerAsync(string game)
        {
            ulong newAuthor = _commandService.Message.Author.Id + 1; // Needs to make sure authors don't match at the first iteration
            SocketUser newAuthorAsSocketUser = _commandService.Message.Author;

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
                    newAuthor = _commandService.Message.Author.Id;
                    newMessage = _commandService.Message.Content;
                }
                // Only executes once so that it sets the user who originally initialized the command
                if (!_commandService.Message.Author.IsBot && originalAuthor == 0 && string.IsNullOrEmpty(originalMessage))
                {
                    originalAuthor = _commandService.Message.Author.Id;
                    originalMessage = _commandService.Message.Content;
                }

                // Builds the embed and sends it to the channel
                if (index.Equals(0) && !jsonResult.Count.Equals(0))
                {
                    Embed embed = await _embed.Build(_commandService, jsonResult, "Result of steam search:");
                    await _commandService.Message.Channel.SendMessageAsync(text: "Which one?", embed: embed);
                    firstDisposableMessage = _commandService.Message.Id;
                    index = 1;
                }

                // Checks if there's nothing in the result, break out of the loop
                if (jsonResult.Count.Equals(0))
                {
                    reply = "Game not found. Weird.";
                    break;
                }

                // Will only execute if we got a reply from the same user
                if (originalAuthor != 0 && originalAuthor.Equals(newAuthor) && !newAuthorAsSocketUser.IsBot && !originalMessage.Equals(newMessage))
                {
                    // Checks if it's a number and if the number is in the list
                    if (int.TryParse(_commandService.Message.Content, out int answer))
                    {
                        secondDisposableMessage = _commandService.Message.Id;
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
