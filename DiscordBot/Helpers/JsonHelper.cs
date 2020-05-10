using Newtonsoft.Json.Linq;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Handlers
{
    //TODO: Make interface, generalize
    public class JsonHelper : IJsonHelper
    {
        public async Task<List<(string, string)>> Parse(string json)
        {
            JObject data = JObject.Parse(json);
            List<(string, string)> itemList = new List<(string, string)>();

            await Task.Run(() =>
            {
                if (data["items"] != null)
                {
                    foreach (JObject item in data["items"])
                    {
                        // Need to be sure it's a valid steam game link
                        if (item["link"].ToString().StartsWith("https://store.steampowered.com/app/"))
                        {
                            string name = item["pagemap"]["product"].First()["name"].ToString();

                            itemList.Add((name, item["link"].ToString()));
                        }
                    }
                }
            });
            return itemList;
        }
    }
}
