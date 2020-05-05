using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Delete : ModuleBase<SocketCommandContext>
    {
        [Command("delete", RunMode = RunMode.Async)]
        [Name("delete <amount>")]
        [Summary("Deletes a specified amount of messages based on the message ID provided.")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task DeleteAsync(ulong num)
        {
            string x = "";
            int counter = 0;
            try
            {
                await Task.Run(async () =>
                {
                    Task<IMessage> numMsg = null;
                    await Task.Run(() => numMsg = Context.Channel.GetMessageAsync(num));

                    if (numMsg.Result?.Content.Length > 0)
                    {
                        ulong latestMessageId = 0;
                        do
                        {
                            latestMessageId = Context.Channel.GetMessagesAsync().LastAsync().Result.First().Id;
                            await (Context.Channel as SocketTextChannel)?.DeleteMessageAsync(latestMessageId);
                            counter++;
                            x = $"Successfully deleted {counter} messages.";
                        } while (latestMessageId != num);
                    }
                    else
                        x = "There is no message with that ID.";
                });
            }
            catch (Exception ex)
            {
                x = ex.Message;
            }
            finally
            {
                await ReplyAsync(x);
            }
        }
    }
}
