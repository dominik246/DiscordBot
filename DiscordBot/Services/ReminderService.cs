using Discord;
using Discord.WebSocket;

using DiscordBot.DiscordBot.Helpers;
using DiscordBot.DiscordBot.Helpers.JsonHelpers;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class ReminderService
    {
        private string _jsonPath;

        private readonly JsonConstructor _builder;
        private readonly IGetDateFromString _getDate;
        private readonly IEmbedHelper _embed;
        private readonly DiscordSocketClient _client;
        private readonly TimerHelper _timer;

        public ReminderService(JsonConstructor builder, IGetDateFromString getDate, IEmbedHelper embed, DiscordSocketClient client, TimerHelper timer)
        {
            _getDate = getDate;
            _builder = builder;
            _embed = embed;
            _client = client;
            _timer = timer;
            _client.LoggedIn += TimerStarter;
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
            await _timer.SetTimer(message, UTCendDate);
            return offsetTimeSpan.Item1;
        }

        public async Task<Embed> GetReminders(SocketUserMessage message)
        {
            string solutionFolderPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string filePath = solutionFolderPath + "\\DiscordBot\\json\\reminder_storage.json";
            _jsonPath = filePath;

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

        private Task OnReminderFinished()
        {

            //_dm.SendDm(client: _client, userId: );

            return Task.CompletedTask;
        }

        private async Task TimerStarter()
        {
            if (File.Exists(_jsonPath))
            {
                string jsonAsString = await File.ReadAllTextAsync(_jsonPath);
                List<JObject> list = new List<JObject>();
                JObject json = JObject.Parse(jsonAsString);
                
                foreach (JObject userId in json["reminders"]["users"])
                {
                    list.Add(userId["UTCendDate"] as JObject);
                }

                //TODO: maybe sort?
                //List<JObject> sortedList = list.OrderBy(o => o["UTCendDate"]).ToList();
                
                /*list.Sort((i1, i2) =>
                Convert.ToDateTime(i1["UTCendDate"]).CompareTo(Convert.ToDateTime(i2)));*/

                
            }
        }
    }
}
