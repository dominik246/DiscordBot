using Newtonsoft.Json.Linq;

using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Handlers
{
    public interface IJsonHelper
    {
        Task<JArray> Parse(string json);
    }
}