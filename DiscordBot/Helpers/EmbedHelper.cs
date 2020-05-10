using Discord;

using DiscordBot.Commands;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class EmbedHelper : IEmbedHandler
    {
        public async Task<string> Build(CommandHandlingService commandService, List<(string, string)> content)
        {
            int index = 0;

            // Checks if we got any result back
            if (content?.Count > 0)
            {
                EmbedBuilder embed = new EmbedBuilder();

                //await ReplyAsync("Which one?");
                await Task.Run(async () =>
                {
                    embed.WithTitle("Result of steam search:");
                    foreach ((string, string) s in content)
                    {
                        embed.AddField($"{++index}) ", s.Item1);
                    }
                    embed.WithColor(Color.Green);
                    await commandService.Message.Channel.SendMessageAsync(text: "Which one?", embed: embed.Build());
                });
                return "";
            }
            else
            {
                return "Game not found. Weird.";
            }
        }
    }
}
