using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class ClientStatusService
    {
        private readonly DiscordSocketClient _client;

        public ClientStatusService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task<string> ChangeAvailability()
        {
            string reply = "Changing bot status to: ";

            if (_client.Status.Equals(UserStatus.Online))
            {
                reply += "Invisible.";
                await _client.SetStatusAsync(UserStatus.Invisible);
            }
            else
            {
                reply += "Online.";
                await _client.SetStatusAsync(UserStatus.Online);
            }
            Console.WriteLine(reply);
            return reply;
        }
    }
}
