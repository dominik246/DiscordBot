using Discord;
using Discord.WebSocket;

using DiscordBot.DiscordBot.Helpers;
using DiscordBot.DiscordBot.Helpers.JsonHelpers;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class ReminderService
    {
        private readonly JsonConstructor _builder;
        private readonly IGetDateFromString _getDate;
        private readonly IEmbedHelper _embed;

        public ReminderService(JsonConstructor builder, IGetDateFromString getDate, IEmbedHelper embed)
        {
            _getDate = getDate;
            _builder = builder;
            _embed = embed;
        }

        public async Task<TimeSpan> SetReminder(SocketUserMessage message, string messageString)
        {
            (TimeSpan, string) offsetTimeSpan = await _getDate.GetTimeSpan(messageString);
            DateTime UTCendDate = message.Timestamp.UtcDateTime + offsetTimeSpan.Item1;
            DateTime localEndDate = message.Timestamp.LocalDateTime + offsetTimeSpan.Item1;


            List<(string, string)> list = new List<(string, string)>()
            {
                ("authorId", message.Author.Id.ToString()),
                ("message", offsetTimeSpan.Item2),
                ("localStartDate", message.Timestamp.LocalDateTime.ToString()),
                ("localEndDate", localEndDate.ToString()),
                ("UTCstartTime", message.Timestamp.UtcDateTime.ToString()),
                ("UTCendDate", UTCendDate.ToString())
            };

            List<string> path = new List<string>()
            {
                "reminders", "users", message.Author.Id.ToString(), message.Id.ToString()
            };

            await _builder.Construct(path, list);
            return offsetTimeSpan.Item1;
        }

        //TODO: make a GetReminders func
        public async Task<Embed> GetReminders(SocketUserMessage message)
        {
            string solutionFolderPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string filePath = solutionFolderPath + "\\DiscordBot\\json\\reminder_storage.json";

            List<(string, string)> list = new List<(string, string)>();
            JObject json = JObject.Parse(await File.ReadAllTextAsync(filePath));

            json = json["reminders"]["users"][message.Author.Id.ToString()] as JObject;

            foreach (var o in json)
            {
                var endDateValue = o.Value.SelectToken("localEndDate").ToString();
                var jMessage = o.Value.SelectToken("message").ToString();
                list.Add((endDateValue, jMessage));
            }

            return await _embed.Build(list, "Reminders:");
        }
    }
}
