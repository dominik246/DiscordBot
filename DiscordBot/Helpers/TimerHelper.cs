using Discord;
using Discord.WebSocket;
using DiscordBot.DiscordBot.Services;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DiscordBot.DiscordBot.Helpers
{
    public class TimerHelper
    {
        private ulong _userId;
        private Embed _embedFromMessage;

        private readonly IDmHelper _dm;
        private readonly DiscordSocketClient _client;
        private readonly IEmbedHelper _embed;

        public TimerHelper(IDmHelper dm, DiscordSocketClient client, IEmbedHelper embed)
        {
            _dm = dm;
            _client = client;
            _embed = embed;
        }

        public async Task SetTimer(SocketUserMessage message, DateTime finishTime)
        {
            TimeSpan timeSpan = finishTime.Subtract(DateTime.UtcNow);
            _userId = message.Author.Id;

            Timer timer = new Timer
            {
                Interval = timeSpan.TotalMilliseconds,
                Enabled = true
            };
            timer.Elapsed += Timer_Elapsed;

            List<(string, string)> list = new List<(string, string)>()
            {
                ("Reminder: ", message.Content)
            };
            _embedFromMessage = await _embed.Build(list);

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _dm.SendDm(client: _client, userId: _userId, embed: _embedFromMessage);
        }

        public async Task Init()
        {
            
        }
    }
}
