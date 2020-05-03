using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Info : ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        [Summary("Returns info about the current user, or the user parameter, if one is passed.")]
        public async Task UserInfoAsync([Summary("The (optional) user to get info about")] SocketUser user = null)
        {
            var userInfo = user ?? Context.User;
            await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        }
    }
}
