using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class LogService
    {
        private readonly IDmOwnerHelper _dm;
        private readonly DiscordSocketClient _client;
        private readonly IEmbedHelper _embed;
        private readonly ILoggerHelper _handler;
        public LogService(IDmOwnerHelper dm, DiscordSocketClient client, ILoggerHelper handler, IEmbedHelper embed)
        {
            _dm = dm;
            _client = client;
            _embed = embed;
            _handler = handler;
        }

        public async Task Log(LogMessage msg)
        {
            await _handler.Log(msg);

            List<(string, string)> list = new List<(string, string)>
                {
                    ("Exception: ", msg.Exception.Message)
                };

            Embed embed = await _embed.Build(list);

            await _dm.SendDm(_client, msg, embed);
        }
    }
}
