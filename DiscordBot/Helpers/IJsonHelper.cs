using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Handlers
{
    public interface IJsonHelper
    {
        Task<List<(string, string)>> Parse(string json);
    }
}