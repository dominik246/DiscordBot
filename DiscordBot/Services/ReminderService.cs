using DiscordBot.DiscordBot.Helpers;
using DiscordBot.DiscordBot.Helpers.JsonHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class ReminderService
    {
        private JsonBuilderHelper _builder;

        public ReminderService(JsonBuilderHelper builder)
        {
            _builder = builder;
        }
        // TODO: check if the user forgot to type a timespan or mistyped it
        public async Task SetReminder(string message, ulong authorId, DateTime startTime)
        {
            string[] messageArray = message.Split(separator: ' ', options: StringSplitOptions.RemoveEmptyEntries);
            List<string> timeList = new List<string>();

            foreach (string result in messageArray.Last().SplitAndKeepDelimiters(new char[] { 'd', 'h', 'm', 's' }))
            {
                timeList.Add(result);
            }

            int test = messageArray.Last().Length;
            message = message[..^(messageArray.Last().Length + 1)];

            // 12d,11h,10m,09s

            int days = 0;
            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            await Task.Run(() =>
            {
                for (int i = 0; i < timeList.Count; i++)
                {
                    string entry = timeList[i];

                    if (entry.EndsWith('d'))
                    {
                        // -1 because you want to trim the last character in the string
                        days = int.Parse(entry[0..(entry.Length - 1)]);
                    }
                    if (entry.EndsWith('h'))
                    {
                        hours = int.Parse(entry[0..(entry.Length - 1)]);
                    }
                    if (entry.EndsWith('m'))
                    {
                        minutes = int.Parse(entry[0..(entry.Length - 1)]);
                    }
                    if (entry.EndsWith('s'))
                    {
                        seconds = int.Parse(entry[0..(entry.Length - 1)]);
                    }
                }
            });

            TimeSpan offsetTimeSpan = new TimeSpan(days, hours, minutes, seconds);
            DateTime finishDate = startTime + offsetTimeSpan;
            await _builder.Build(author: authorId, message: message, finishDateTime: finishDate, startDateTime: startTime);
        }
    }
}
