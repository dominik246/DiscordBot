using Discord;

using DiscordBot.Commands;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class EmbedHelper : IEmbedHelper
    {
        public async Task<Embed> Build(CommandHandlingService commandService, List<(string, string)> content, string title)
        {
            int index = 0;

            // Checks if we got any result back
            if (content?.Count < 1)
                return new EmbedBuilder().AddField("Result: ", "Nothing has been found.").Build();

            // Otherwise build the embed
            EmbedBuilder embed = new EmbedBuilder();

            await Task.Run(() =>
            {
                embed.WithTitle(title);
                foreach ((string, string) s in content)
                {
                    embed.AddField($"{++index}) ", s.Item1);
                }
                embed.WithColor(Color.Green);
                //await commandService.Message.Channel.SendMessageAsync(text: "Which one?", embed: embed.Build());
            });
            return embed.Build();
        }
    }
}
