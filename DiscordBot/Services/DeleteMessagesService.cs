using Discord;

using DiscordBot.Commands;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class DeleteMessagesService
    {
        public async Task<string> DeleteTaskAsync(CommandHandlingService commandService, ulong id, int count)
        {
            string reply = "";
            await Task.Run(async () =>
            {
                int deleteCounter = 0;
                ulong latestMessageId = 0;
                IMessage msgId = await commandService.Message.Channel.GetMessageAsync(id);
                IReadOnlyCollection<IMessage> getLast100Messages = await commandService.Message.Channel.GetMessagesAsync().LastAsync();

                // Ensures that message length of numMsg is greater than 0, therefore ensures that the message exists
                if (msgId?.Content.Length > 0)
                {
                    do
                    {
                        // Automagically gets the latest message from the channel and deletes it
                        latestMessageId = getLast100Messages.ElementAt(deleteCounter).Id;
                        await getLast100Messages.ElementAt(deleteCounter).DeleteAsync();
                        Thread.Sleep(500);
                        deleteCounter++;
                    } while (latestMessageId != id);

                    reply = $"Successfully deleted {deleteCounter} messages.";
                }
                else
                {
                    foreach (IMessage msg in getLast100Messages)
                    {
                        ulong authorId = msg.Author.Id;
                        ulong resultMsg = msg.Id;

                        if (authorId.Equals(id))
                        {
                            await commandService.Message.Channel.DeleteMessageAsync(resultMsg);
                            Thread.Sleep(500);
                            deleteCounter++;
                        }
                        if (count.Equals(deleteCounter))
                        {
                            // Counter is still by 1 short because it iterates from 0
                            deleteCounter++;
                            break;
                        }
                    }
                    reply = $"Successfully deleted {deleteCounter + 1} messages.";
                }
                if (deleteCounter == 0)
                {
                    reply = "0 messages deleted. Have you entered a valid message/user ID?";
                }
            });
            return reply;
        }
    }
}
