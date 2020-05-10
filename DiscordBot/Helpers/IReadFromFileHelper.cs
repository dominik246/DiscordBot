using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Handlers
{
    public interface IReadFromFileHelper
    {
        Task<List<string>> ReadAsync(uint count);
    }
}