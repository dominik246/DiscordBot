using Discord;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class EmbedHelper : IEmbedHelper
    {
        public async Task<Embed> Build(List<(string, string)> content, string title, bool inline)
        {
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
                    embed.AddField(s.Item1, s.Item2, inline);
                }
                embed.WithColor(Color.Green);
            });
            return embed.Build();
        }
    }
}
