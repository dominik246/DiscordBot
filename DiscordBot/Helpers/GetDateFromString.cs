using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Helpers
{
    public class GetDateFromString : IGetDateFromString
    {
        // TODO: check if the user forgot to type a timespan or mistyped it
        public async Task<(TimeSpan, string)> GetTimeSpan(string message)
        {
            string[] messageArray = message.Split(separator: ' ', options: StringSplitOptions.RemoveEmptyEntries);
            List<string> timeList = new List<string>();

            foreach (string result in messageArray.Last().SplitAndKeepDelimiters(new char[] { 'd', 'h', 'm', 's' }))
            {
                timeList.Add(result);
            }

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
                        // trimming the last character from the string
                        days = int.Parse(entry[..(entry.Length - 1)]);
                    }
                    if (entry.EndsWith('h'))
                    {
                        hours = int.Parse(entry[..(entry.Length - 1)]);
                    }
                    if (entry.EndsWith('m'))
                    {
                        minutes = int.Parse(entry[..(entry.Length - 1)]);
                    }
                    if (entry.EndsWith('s'))
                    {
                        seconds = int.Parse(entry[..(entry.Length - 1)]);
                    }
                }
            });

            return (new TimeSpan(days, hours, minutes, seconds), message);
        }
    }
}
