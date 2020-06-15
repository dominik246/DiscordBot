using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.DiscordBot.Services;
using System;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Commands
{
    public class Vanish : ModuleBase<SocketCommandContext>
    {
        private readonly ClientStatusService _service;

        public Vanish(ClientStatusService service)
        {
            _service = service;
        }

        [Command("vanish")]
        [Name("vanish")]
        [Summary("Gets offline or online")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task VanishAsync()
        {
            Console.WriteLine(await _service.ChangeAvailability());
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
        }
    }
}
