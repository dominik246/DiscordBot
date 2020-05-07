using Discord;
using Discord.Commands;

using DiscordBot.DiscordBot.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class DeleteMessages : ModuleBase<SocketCommandContext>
    {
        private readonly CommandHandlingService _commandService;
        public DeleteMessages(CommandHandlingService commandService)
        {
            _commandService = commandService;
        }

        [Command("delete", RunMode = RunMode.Async)]
        [Name("delete <amount>/<untilId>/<userId> [optional]<amount>")]
        [Summary("Deletes a specified amount of messages based on the ID or amount provided.")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task DeleteAsync(ulong num, int amount = 100)
        {
            string reply = "";
            DeleteMessagesService dms = new DeleteMessagesService();
            try
            {
                // If the user types a count and not an ID, it converts the count to the specified ID and continues
                if (num <= 100)
                {
                    IReadOnlyCollection<IMessage> message = await Context.Channel.GetMessagesAsync((int)num + 1).LastAsync();
                    num = message.Last().Id;
                }
                reply = await dms.DeleteTaskAsync(_commandService, num, amount);
            }
            catch (Exception ex)
            {
                reply = ex.Message;
            }
            finally
            {
                await ReplyAsync(reply);
            }
        }
    }
}
