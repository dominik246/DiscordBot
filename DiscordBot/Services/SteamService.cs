using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class SteamService
    {
        public async Task<List<string>> GetInfo(string userInput, bool restrict = false)
        {
            // Google API key, you can get it here: https://console.developers.google.com/apis/credentials
            // -> click "Create Credentials"
            // -> "API Key"
            string apiKey = Environment.GetEnvironmentVariable("GoogleAPIToken");

            // Google CSE Token, you can get it here: https://cse.google.com/cse/all
            // -> click "Add"
            // -> fill in "Sites to search"
            // -> when created, open the new CSE link and under "Search engine ID" click "Copy to clipboard"
            string cseToken = Environment.GetEnvironmentVariable("GoogleCSEToken");
            
            // Because we use google search engine, it doesn't like when you type spaces and hashtags,
            // so we have to take care we don't input that
            string queryTerm = userInput.Replace(' ', '+').Replace("#", "%23");

            // Ensuring we only get applications from steam, and not tags, search terms, franchises or anything else
            string querySuffix = "+site:store.steampowered.com/app/";

            // Siterestrict is for the ppl who want to pay for query searches, I don't see a reason why to
            string suffix = "/siterestrict";
            string address = "https://www.googleapis.com/customsearch/v1";
            string json = "";
            string fullAddress = address + (restrict ? suffix : "") + "?key=" + apiKey + "&cx=" + cseToken +
                "&q=" + queryTerm + querySuffix;

            using (HttpClient hc = new HttpClient())
            {
                await Task.Run(() => json = hc.GetStringAsync(fullAddress).Result);
                return JsonParser(json).Result;
            }
        }

        private async Task<List<string>> JsonParser(string json)
        {
            JObject data = JObject.Parse(json);
            List<string> itemList = new List<string>();

            await Task.Run(() =>
            {
                if (data["items"] != null)
                {
                    foreach (JObject item in data["items"])
                    {
                        // Need to be sure it's a valid steam game link
                        if (item["link"].ToString().StartsWith("https://store.steampowered.com/app/"))
                        {
                            itemList.Add(item["link"].ToString());
                            break;
                        }
                    }
                }
            });

            return itemList;
        }
    }
}
