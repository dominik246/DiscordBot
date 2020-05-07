using Discord;

using DiscordBot.Commands;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    class DeleteMessagesService
    {
        public async Task<string> DeleteTaskAsync(CommandHandlingService commandService, ulong num, int n)
        {
            string reply = "";
            await Task.Run(async () =>
            {
                int counter = 0;
                ulong latestMessageId = 0;
                IMessage msgId = await commandService.Message.Channel.GetMessageAsync(num);
                IReadOnlyCollection<IMessage> getLast100Messages = await commandService.Message.Channel.GetMessagesAsync().LastAsync();

                // Ensures that message length of numMsg is greater than 0, therefore ensures that the message exists
                if (msgId?.Content.Length > 0)
                {
                    do
                    {
                        // Automagically gets the latest message from the channel and deletes it
                        latestMessageId = getLast100Messages.ElementAt(counter).Id;
                        await getLast100Messages.ElementAt(counter).DeleteAsync();
                        Thread.Sleep(500);
                        counter++;
                    } while (latestMessageId != num);

                    reply = $"Successfully deleted {counter} messages.";
                }
                else
                {
                    foreach (IMessage msg in getLast100Messages)
                    {
                        ulong authorId = msg.Author.Id;
                        ulong resultMsg = msg.Id;

                        if (authorId.Equals(num))
                        {
                            await commandService.Message.Channel.DeleteMessageAsync(resultMsg);
                            Thread.Sleep(500);
                            counter++;
                        }
                        if (n.Equals(counter))
                        {
                            // Counter is still by 1 short because it iterates from 0
                            counter++;
                            break;
                        }
                    }
                    reply = $"Successfully deleted {counter + 1} messages.";
                }
                if (counter == 0)
                {
                    reply = "0 messages found. Have you entered a valid message/user ID?";
                }
            });
            return reply;
        }
    }
}
