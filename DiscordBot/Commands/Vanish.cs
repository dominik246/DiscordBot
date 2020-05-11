using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
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

        //TODO: make Service class
        [Command("vanish")]
        [Name("vanish")]
        [Summary("Gets offline or online")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task VanishAsync()
        {
            string reply = "Changing bot status to: ";
            ulong originalMessageId = Context.Message.Id;
            bool isCompleted = false;

            if (_client.Status.Equals(UserStatus.Online) && !isCompleted)
            {
                isCompleted = true;
                reply += "Invisible";
                await _client.SetStatusAsync(UserStatus.Invisible);
            }
            if (_client.Status.Equals(UserStatus.Invisible) && !isCompleted)
            {
                isCompleted = true;
                reply += "Online";
                await _client.SetStatusAsync(UserStatus.Online);
            }
            Console.WriteLine(reply);
            await Context.Channel.DeleteMessageAsync(originalMessageId);
        }
    }
}
