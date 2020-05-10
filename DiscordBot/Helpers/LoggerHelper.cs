using Discord;

using System;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class LoggerHelper : ILoggerHelper
    {
        public Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
