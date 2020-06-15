using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    //TODO: check if the embed is larger than 2000 chars, if yes, split it
    public class DmHelper : IDmHelper
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
        
        public async Task SendDm(DiscordSocketClient client, ulong userId, Embed embed)
        {
            Console.WriteLine("Sending message to owner!");
            await client.GetUser(userId).SendMessageAsync(embed: embed);
        }
    }
}
