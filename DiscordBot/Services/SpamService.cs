using DiscordBot.Commands;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    class SpamService
    {
        private static Random random = new Random();
        private async Task<List<string>> GetRandomString()
        {
            string solutionFolderPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            FileStream fs = new FileStream(solutionFolderPath + "//DiscordBot//json//shakespeare_6.0_unformated_fixed.json.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string line = "";

            // determine extent of source file
            long lastPos = sr.BaseStream.Seek(0, SeekOrigin.End);

            {
                // generate a random position
                double pct = random.NextDouble(); // [0.0, 1.0)
                long randomPos = (long)(pct * lastPos);
                if (pct >= 0.99)
                    randomPos -= (long)(lastPos * 0.99); // if near the end, back up a bit

                sr.BaseStream.Seek(randomPos, SeekOrigin.Begin);

                line = sr.ReadLine(); // consume current partial line
                line = sr.ReadLine(); // this will be a full line
                sr.DiscardBufferedData(); // magic

                // extract info from line here
            }

            sr.Close();
            fs.Close();

            List<string> linesList = new List<string>();

            await Task.Run(() =>
            {
                int maxIndex = 127;
                while (line.Length > 0)
                {
                    if (line.Length < maxIndex)
                    {
                        maxIndex = line.Length - 1;
                    }

                    while (char.IsLetterOrDigit(line[maxIndex]) && (maxIndex < line.Length))
                    {
                        maxIndex++;
                    }

                    linesList.Add(line[0..(maxIndex + 1)]);
                    line = line.Remove(0, maxIndex + 1);
                }
            });
            return linesList;
        }

        public async Task SpamString(CommandHandlingService commandService, uint count)
        {
            List<string> linesList = new List<string>();
            await Task.Run(() =>
            {
                while (linesList.Count < count)
                {
                    foreach (string s in GetRandomString().Result)
                    {
                        linesList.Add(s);
                    }
                }
            });

            await Task.Run(async () =>
            {
                foreach (string line in linesList)
                {
                    string prefix = $"";

                    await commandService.Message.Channel.SendMessageAsync(prefix + line);
                    Thread.Sleep(1000);
                }
            });
        }
    }
}
