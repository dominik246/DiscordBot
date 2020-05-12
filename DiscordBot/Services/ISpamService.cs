
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public interface ISpamService
    {
        Task SpamString(uint count);
    }
}