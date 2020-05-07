using Discord;
using Discord.Commands;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class GetMessage : ModuleBase<SocketCommandContext>
    {
        [Command("getn")]
        [Summary("Gets the Nth message.")]
        [Name("getn <value>")]
        [RequireBotPermission(GuildPermission.ViewChannel)]
        public async Task GetNthMessage(int num)
        {
            if (num > 100)
            {
                await ReplyAsync("Number has to be less than 101");

            }
            else
            {
                IReadOnlyCollection<IMessage> messages = await Context.Channel.GetMessagesAsync(num).LastAsync();
                string result = messages.Last().GetJumpUrl();

                await ReplyAsync(result);
            }
        }
    }
}
