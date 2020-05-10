using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Handlers
{
    public interface IGoogleApiHelper
    {
        Task<string> GetResult(string userInput, bool restrict = false);
    }
}