using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Helpers.JsonHelpers
{
    public class JsonBuilderHelper
    {
        public async Task Build(ulong author, string message, DateTime finishDateTime, DateTime startDateTime)
        {

            ReminderConstructorHelper reminder = new ReminderConstructorHelper()
            {
                AuthorId = author,
                Message = message,
                FinishDateTime = finishDateTime,
                StartDateTime = startDateTime
            };

            // TODO: make a dedicated class for FileStream
            string solutionFolderPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string filePath = solutionFolderPath + "\\DiscordBot\\json\\reminder_storage.json";
            FileStream fs = new FileStream(path: filePath, mode: FileMode.OpenOrCreate,
                access: FileAccess.ReadWrite, share: FileShare.Read, bufferSize: 4096, options: FileOptions.Asynchronous);
            StreamWriter sw = new StreamWriter(fs, bufferSize: 4096);
            JsonTextWriter writer = new JsonTextWriter(sw)
            {
                Formatting = Formatting.Indented
            };

            // TODO: FIXME: make a separate function when the file already exists
            JObject jReminder = new JObject(
            new JObject(
                new JProperty("reminders",
                    new JObject(
                        new JProperty("users",
                            new JObject(
                                new JProperty(author.ToString(),
                                    new JObject(
                                        new JProperty("author", reminder.AuthorId),
                                        new JProperty("message", reminder.Message),
                                        new JProperty("finishDateTime", reminder.FinishDateTime),
                                        new JProperty("startDateTime", reminder.StartDateTime)))))))));

            await jReminder.WriteToAsync(writer);
            await writer.CloseAsync();

            await Task.Run(() =>
            {
                sw.Close();
                fs.Close();
            });
        }
    }
}
