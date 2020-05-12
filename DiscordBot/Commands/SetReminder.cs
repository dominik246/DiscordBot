using Discord.Commands;

using DiscordBot.DiscordBot.Services;

using System;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Commands
{
    public class SetReminder : ModuleBase<SocketCommandContext>
    {
        private readonly ReminderService _service;
        public SetReminder(ReminderService service)
        {
            _service = service;
        }

        [Command("reminder")]
        [Summary("setReminder")]
        public async Task ReminderCommand([Summary("The given input")][Remainder] string message)
        {
            TimeSpan result = await _service.SetReminder(Context.Message, message);
            string days = result.Days == 0 ? "" : result.Days + " days ";
            days = days == "1 days " ? "1 day " : "1 days ";
            string hours = result.Hours == 0 ? "" : result.Hours + " hours ";
            string minutes = result.Minutes == 0 ? "" : result.Minutes + " minutes ";
            string seconds = result.Seconds == 0 ? "" : result.Seconds + " seconds";
            string offset = days + hours + minutes + seconds;

            await ReplyAsync($"Done. See you in {offset}.");
        }
    }
}
