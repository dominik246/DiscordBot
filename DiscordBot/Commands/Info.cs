using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.DiscordBot.Services;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Info : ModuleBase<SocketCommandContext>
    {
        [Command("info", RunMode = RunMode.Async)]
        [Summary("Returns info about the current user, or the user parameter, if one is passed.")]
        public async Task UserInfoAsync([Summary("The (optional) user to get info about")] SocketUser user = null)
        {
            user ??= Context.User;

            await ReplyAsync($"{user.Username}#{user.Discriminator}");
        }
    }
}
