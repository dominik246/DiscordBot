using Discord;
using Discord.WebSocket;

using DiscordBot.Commands;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class SteamService
    {
        private async Task<List<(string, string)>> GetInfo(string userInput, bool restrict = false)
        {
            // Google API key, you can get it here: https://console.developers.google.com/apis/credentials
            // -> click "Create Credentials"
            // -> "API Key"
            string apiKey = Environment.GetEnvironmentVariable("GoogleAPIToken");

            // Google CSE Token, you can get it here: https://cse.google.com/cse/all
            // -> click "Add"
            // -> fill in "Sites to search"
            // -> when created, open the new CSE link and under "Search engine ID" click "Copy to clipboard"
            string cseToken = Environment.GetEnvironmentVariable("GoogleCSEToken");

            // Because we use google search engine, it doesn't like when you type spaces and hashtags,
            // so we have to take care we don't input that
            string queryTerm = userInput.Replace(' ', '+').Replace("#", "%23");

            // Ensuring we only get applications from steam, and not tags, search terms, franchises or anything else
            string querySuffix = "+site:store.steampowered.com/app/";

            // Siterestrict is for the ppl who want to pay for query searches, I don't see a reason why to
            string suffix = "/siterestrict";
            string address = "https://www.googleapis.com/customsearch/v1";
            string json = "";
            string fullAddress = address + (restrict ? suffix : "") + "?key=" + apiKey + "&cx=" + cseToken +
                "&q=" + queryTerm + querySuffix;
            Console.WriteLine(fullAddress);

            using (HttpClient hc = new HttpClient())
            {
                await Task.Run(() => json = hc.GetStringAsync(fullAddress).Result);
                return JsonParser(json).Result;
            }
        }

        private async Task<List<(string, string)>> JsonParser(string json)
        {
            JObject data = JObject.Parse(json);
            List<(string, string)> itemList = new List<(string, string)>();

            await Task.Run(() =>
            {
                if (data["items"] != null)
                {
                    foreach (JObject item in data["items"])
                    {
                        // Need to be sure it's a valid steam game link
                        if (item["link"].ToString().StartsWith("https://store.steampowered.com/app/"))
                        {
                            string name = item["pagemap"]["product"].First()["name"].ToString();

                            itemList.Add((name, item["link"].ToString()));
                        }
                    }
                }
            });

            return itemList;
        }

        public async Task<(ulong, ulong, string)> GetAnswerAsync(CommandHandlingService commandService, string game)
        {
            EmbedBuilder eb = new EmbedBuilder();

            ulong newAuthor = commandService.Message.Author.Id + 1; // Needs to make sure authors don't match at the first iteration
            SocketUser newAuthorAsSocketUser = commandService.Message.Author;

            string reply = "";
            int index = 0;
            ulong originalAuthor = 0;
            string originalMessage = "";
            string newMessage = "";
            ulong firstDisposableMessage = 0;
            ulong secondDisposableMessage = 0;

            // Searches for possible matches on steam and returns a list of them
            List<(string, string)> result = await GetInfo(game);

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
                            await commandService.Message.Channel.SendMessageAsync(text: "Which one?", embed: eb.Build());
                            //await ReplyAsync(message: "Which one?", embed: eb.Build());
                            firstDisposableMessage = commandService.Message.Id;
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
                    if (int.TryParse(commandService.Message.Content, out int answer))
                    {
                        secondDisposableMessage = commandService.Message.Id;
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
            return (firstDisposableMessage, secondDisposableMessage, reply);
        }
    }
}
