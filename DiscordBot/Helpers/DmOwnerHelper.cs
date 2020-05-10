using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    //TODO: make interface
    public class DmOwnerHelper : IDmOwnerHelper
    {
        public async Task SendDm(DiscordSocketClient client, LogMessage message)
        {
            if (!(message.Exception is CommandException commandException))
                return;

            if (ulong.TryParse(Environment.GetEnvironmentVariable("OwnerId"), out ulong ownerId))
            {
                Console.WriteLine("Sending message to owner!");
                await client.GetUser(ownerId).SendMessageAsync(message.Exception.Message); //TODO: change to embed
            }

            Console.WriteLine($"{commandException.Context.User} failed to execute " +
                $"'{commandException.Command.Name}' in '{commandException.Context.Channel}'.");
            Console.WriteLine(commandException.ToString());
            await commandException.Context.Channel.SendMessageAsync(message.Message);
        }
    }
}
