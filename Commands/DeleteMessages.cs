using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class DeleteMessages : ModuleBase<SocketCommandContext>
    {
        [Command("delete", RunMode = RunMode.Async)]
        [Name("delete <amount>")]
        [Summary("Deletes a specified amount of messages based on the message ID provided.")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task DeleteAsync(ulong num)
        {
            string reply = "";
            try
            {
                await Task.Run(async () =>
                {
                    int counter = 0;
                    ulong latestMessageId = 0;
                    IMessage msgId = await Context.Channel.GetMessageAsync(num);
                    IReadOnlyCollection<IMessage> getLast100Messages = await Context.Channel.GetMessagesAsync().LastAsync();

                    // Ensures that message length of numMsg is greater than 0, therefore ensures that the message exists
                    if (msgId.Content.Length > 0)
                    {
                        do
                        {
                            // Automagically gets the latest message from the channel and deletes it
                            latestMessageId = getLast100Messages.ElementAt(counter).Id;
                            await getLast100Messages.ElementAt(counter).DeleteAsync();
                            Thread.Sleep(300);
                            counter++;
                        } while (latestMessageId != num);

                        reply = $"Successfully deleted {counter} messages.";
                    }
                    else
                    {
                        foreach(IMessage msg in getLast100Messages)
                        {
                            ulong authorId = msg.Author.Id;
                            ulong resultMsg = msg.Id;

                            if(authorId.Equals(num))
                            {
                                await Context.Channel.DeleteMessageAsync(resultMsg);
                                counter++;
                                reply = $"Successfully deleted {counter} messages.";
                            }
                        }
                    }
                    if (counter == 0)
                    {
                        reply = "0 messages found. Have you entered a valid message/user ID?";
                    }
                });
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
