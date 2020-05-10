using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class LogService
    {
        private readonly IDmOwnerHelper _dm;
        private readonly DiscordSocketClient _client;
        //private readonly Embed _embed;
        private readonly ILoggerHelper _handler;
        public LogService(IDmOwnerHelper dm, DiscordSocketClient client, ILoggerHelper handler)
        {
            _dm = dm;
            _client = client;
            //_embed = embed;
            _handler = handler;
        }

        public async Task LogClient(LogMessage msg)
        {
            await _handler.Log(msg);
        }

        public async Task LogCommand(LogMessage msg)
        {
            await _handler.Log(msg);
            await _dm.SendDm(_client, msg);
        }
    }
}
