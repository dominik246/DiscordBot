using Discord.Commands;

using DiscordBot.DiscordBot.Services;

using System;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Commands
{
    public class Reminder : ModuleBase<SocketCommandContext>
    {
        private readonly ReminderService _service;
        public Reminder(ReminderService service)
        {
            _service = service;
        }

        //[Command("reminder")]
        public async Task ReminderCommand([Summary("The given input")][Remainder] string message)
        {
            ulong authorId = Context.Message.Author.Id;
            DateTime startTime = Context.Message.Timestamp.UtcDateTime;

            await _service.SetReminder(message, authorId, startTime);
        }
    }
}
