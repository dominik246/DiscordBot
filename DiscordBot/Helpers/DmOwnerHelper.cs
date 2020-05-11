using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class DmOwnerHelper : IDmOwnerHelper
    {
        public async Task SendDm(DiscordSocketClient client, LogMessage message, Embed embed)
        {
            if (!(message.Exception is CommandException commandException))
                return;

            if (ulong.TryParse(Environment.GetEnvironmentVariable("OwnerId"), out ulong ownerId))
            {
                Console.WriteLine("Sending message to owner!");
                await client.GetUser(ownerId).SendMessageAsync(embed: embed);
            }

            await commandException.Context.Channel.SendMessageAsync(message.Message);
        }
    }
}
