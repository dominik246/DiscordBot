using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Handlers
{
    public class GoogleApiHelper : IGoogleApiHelper
    {
        public async Task<string> GetResult(string userInput, bool restrict = false)
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
            string fullAddress = address + (restrict ? suffix : "") + "?key=" + apiKey + "&cx=" + cseToken +
                "&q=" + queryTerm + querySuffix;
            Console.WriteLine(fullAddress);

            using (HttpClient hc = new HttpClient())
            {
                return await hc.GetStringAsync(fullAddress);
            }
        }
    }
}
