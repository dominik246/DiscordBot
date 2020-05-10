using Discord;
using System;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    //TODO: Make interface
    public class LoggerHelper : ILoggerHelper
    {
        public Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
