using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Commands
{
    public class Vanish : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;

        public Vanish(DiscordSocketClient client)
        {
            _client = client;
        }

        [Command("vanish")]
        [Name("vanish")]
        [Summary("Gets offline or online")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task VanishAsync()
        {
            string reply = string.Empty;
            ulong originalMessageId = Context.Message.Id;
            bool isCompleted = false;

            if (_client.Status.Equals(UserStatus.Online) && !isCompleted)
            {
                isCompleted = true;
                Console.WriteLine("Changing bot status to: Invisible");
                await _client.SetStatusAsync(UserStatus.Invisible);
            }
            if (_client.Status.Equals(UserStatus.Invisible) && !isCompleted)
            {
                isCompleted = true;
                Console.WriteLine("Changing bot status to: Online");
                await _client.SetStatusAsync(UserStatus.Online);
            }
            await Context.Channel.DeleteMessageAsync(originalMessageId);
        }
    }
}
