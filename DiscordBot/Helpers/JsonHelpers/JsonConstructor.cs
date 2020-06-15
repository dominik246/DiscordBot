using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Helpers.JsonHelpers
{
    public class JsonConstructor
    {
        // TODO: Maintainability index is 47
        public async Task Construct(List<string> path, List<(string, string)> context)
        {
            string solutionFolderPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string filePath = solutionFolderPath + "\\DiscordBot\\json\\reminder_storage.json";
            string fsContent = await File.ReadAllTextAsync(filePath) ?? string.Empty;

            string output = "";

            await Task.Run(() =>
            {
                JObject json = new JObject();

                if (!fsContent.Equals(string.Empty) && !fsContent.IsValidJson())
                {
                    // First we need to check if there's an existing file there before copying to it
                    int index = 0;
                    string newPath = "";

                    if (File.Exists(filePath))
                    {
                        do
                        {
                            newPath = filePath + ".invalid" + index;
                            index++;
                        } while (File.Exists(newPath));

                        File.Copy(filePath, newPath);
                    }
                }

                if (fsContent.IsValidJson())
                {
                    json = JObject.Parse(fsContent);
                }

                for (int i = 0; i < path.Count; i++)
                {
                    if (!json.ContainsKey(path[i]))
                    {
                        json.Add(path[i], new JObject());
                    }
                    json = json[path[i]] as JObject;
                }

                foreach ((string, string) vp in context)
                {
                    json.Add(vp.Item1, vp.Item2);
                }

                foreach (string s in path)
                {
                    json = json.Root as JObject;
                }
                output = JsonConvert.SerializeObject(json, Formatting.Indented);
            });
            await File.WriteAllTextAsync(filePath, output);
        }
    }
}
