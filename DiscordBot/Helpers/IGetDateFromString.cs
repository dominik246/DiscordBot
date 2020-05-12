using System;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Helpers
{
    public interface IGetDateFromString
    {
        Task<(TimeSpan, string)> GetTimeSpan(string message);
    }
}