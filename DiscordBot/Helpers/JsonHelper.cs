using Newtonsoft.Json.Linq;

using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Handlers
{
    //TODO: generalize
    public class JsonHelper : IJsonHelper
    {
        public async Task<JArray> Parse(string json)
        {
            JArray result = new JArray();
            JObject data = JObject.Parse(json);

            await Task.Run(() =>
            {
                if (data["items"] != null)
                {
                    foreach (JObject item in data["items"])
                    {
                        // Need to be sure it's a valid steam game link
                        if (item["formattedUrl"].ToString().StartsWith("https://store.steampowered.com/app/"))
                        {
                            result.Add(item);
                        }
                    }
                }
            });
            return result;
        }
    }
}
