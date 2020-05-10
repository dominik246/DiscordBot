using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Handlers
{
    //TODO: generalize
    public class ReadFromFileHelper : IReadFromFileHelper
    {
        private List<string> list = new List<string>();

        public async Task<List<string>> ReadAsync(uint counter)
        {
            string solutionFolderPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string filePath = solutionFolderPath + "\\DiscordBot\\json\\shakespeare_6.0_unformated_fixed.json.txt";
            FileStream fs = new FileStream(path: filePath, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.Read, bufferSize: 4096, useAsync: true);
            StreamReader sr = new StreamReader(fs);

            // Checks if the counter is larger than 20
            if (counter > 20)
            {
                counter = 20;
            }

            // Divides the work for the sake of the buffer
            if (counter > 5)
            {
                await Task.Run(async () =>
                {
                    int iterator = Math.DivRem((int)counter, 5, out int reminder);
                    for (int i = 0; i < iterator; i++)
                    {
                        await Run(iterator, sr);
                    }
                    await Run(reminder, sr);
                });
            }
            else
            {
                await Run((int)counter, sr);
            }

            sr.DiscardBufferedData(); // Clears the buffer

            sr.Close();
            fs.Close();

            return list;
        }

        private async Task Run(int counter, StreamReader sr)
        {
            for (int i = 0; i < counter; i++)
            {
                Random random = new Random();

                // determine extent of source file
                long lastPos = sr.BaseStream.Seek(0, SeekOrigin.End);

                // generate a random position
                double pct = random.NextDouble(); // [0.0, 1.0)
                long randomPos = (long)(pct * lastPos);
                if (pct >= 0.99)
                    randomPos -= (long)(lastPos * 0.97); // if near the end, back up a bit

                sr.BaseStream.Seek(randomPos, SeekOrigin.Begin);

                Task<string> taskCompleted = await sr.ReadLineAsync() // consume current partial line
                    .ContinueWith(async delegate { return await sr.ReadLineAsync(); }); // this will be a full line

                await ShortenString(taskCompleted.Result);
            }
        }

        private async Task<List<string>> ShortenString(string line, int maxIndex = 127)
        {
            await Task.Run(() =>
            {
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

                    list.Add(line[0..(maxIndex + 1)]);
                    line = line.Remove(0, maxIndex + 1);
                }
            });

            return list;
        }
    }
}
