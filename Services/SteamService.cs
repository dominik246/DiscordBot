using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class SteamService
    {
        public List<string> GetInfo(string userInput, bool restrict = false)
        {
            string api = Environment.GetEnvironmentVariable("GoogleAPIToken");
            string cx = Environment.GetEnvironmentVariable("GoogleCSE");
            string query = userInput.Replace(' ', '+').Replace("#", "%23");
            string querySuffix = "+site:store.steampowered.com/app";
            string suffix = "/siterestrict";
            string address = "https://www.googleapis.com/customsearch/v1";
            string fullAddress = address + (restrict ? suffix : "") + "?key=" + api + "&cx=" + cx + "&q=" + query + querySuffix;

            using (HttpClient wc = new HttpClient())
            {
                string json = wc.GetStringAsync(fullAddress).Result;
                return JsonParser(json).Result;
            }
        }

        private async Task<List<string>> JsonParser(string json)
        {
            dynamic data = JObject.Parse(json);
            List<string> itemList = new List<string>();

            await Task.Run(() =>
            {
                foreach (dynamic item in data["items"])
                {
                    if (item["link"].ToString().StartsWith("https://store.steampowered.com/app/"))
                    {
                        itemList.Add(item["link"].ToString());
                    }
                }
                Console.WriteLine(itemList[0]);
            });

            return itemList;
        }
    }
}
