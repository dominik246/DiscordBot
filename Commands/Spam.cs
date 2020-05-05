using Discord.Commands;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Spam : ModuleBase<SocketCommandContext>
    {
        [Command("spam", RunMode = RunMode.Async)]
        [Name("spam")]
        [Summary("Spams random stuff in the given channel.")]
        [RequireBotPermission(Discord.GuildPermission.ManageMessages)]
        [RequireUserPermission(Discord.GuildPermission.ManageMessages)]
        public async Task SpamAsync()
        {
            List<string> linesList = GetRandomString();
            foreach (string line in linesList)
            {
                Thread.Sleep(1200);
                string prefix = $"";

                await ReplyAsync(prefix + line);
            }
            await ReplyAsync("``Finished!``");
        }

        private static Random random = new Random();
        private static List<string> GetRandomString()
        {
            string solutionFolderPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            FileStream fs = new FileStream(solutionFolderPath + "//json//shakespeare_6.0_unformated_fixed.json.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string line = "";

            // determine extent of source file
            long lastPos = sr.BaseStream.Seek(0, SeekOrigin.End);

            {
                // generate a random position
                double pct = random.NextDouble(); // [0.0, 1.0)
                long randomPos = (long)(pct * lastPos);
                if (pct >= 0.99)
                    randomPos -= 1024; // if near the end, back up a bit

                sr.BaseStream.Seek(randomPos, SeekOrigin.Begin);

                line = sr.ReadLine(); // consume curr partial line
                line = sr.ReadLine(); // this will be a full line
                sr.DiscardBufferedData(); // magic

                // extract info from line here
            }

            sr.Close();
            fs.Close();

            int maxIndex = 127;
            if (line.Length < maxIndex)
            {
                maxIndex = line.Length - 1;
            }

            List<string> linesList = new List<string>();
            while (line.Length > maxIndex)
            {
                linesList.Add(line[0..maxIndex]);
                line = line.Remove(0, maxIndex);
            }
            return linesList;
        }
    }
}
