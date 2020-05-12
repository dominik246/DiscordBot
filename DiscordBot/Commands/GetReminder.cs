using Discord;
using Discord.Commands;

using DiscordBot.DiscordBot.Services;

using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Commands
{
    public class GetReminder : ModuleBase<SocketCommandContext>
    {
        private readonly ReminderService _service;

        public GetReminder(ReminderService service)
        {
            _service = service;
        }

        [Command("getreminders")]
        public async Task Reminder()
        {
            Embed embed = await _service.GetReminders(Context.Message);

            await Context.Message.Author.SendMessageAsync(embed: embed);
        }
    }
}
