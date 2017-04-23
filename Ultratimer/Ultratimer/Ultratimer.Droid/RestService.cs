using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ultratimer;
using System.IO;
using System.Net;

namespace Ultratimer.Droid
{
    class RestService: IRestService
    {
        HttpClient client;
        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }
        public async Task<List<Game>> GetGamesAsync()
        {
            try { 
            string RestUrl = "http://nfcmai.azurewebsites.net/api/TreasureHuntGames";
            var uri = new Uri(string.Format(RestUrl));
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<Game> Items = JsonConvert.DeserializeObject<List<Game>>(content);
                return Items;
            }
            }
            catch { }
            return null;
        }
        public async Task<Clues> GetFirstClueAndStoreNameAsync(int gameId, string participant)
        {
            try
            {
                string RestUrl = "http://nfcmai.azurewebsites.net/api/TreasureHuntTips?gameId={0}&participant={1}";
                var uri = new Uri(string.Format(RestUrl,gameId,participant));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Clues Items = JsonConvert.DeserializeObject<Clues>(content);
                    return Items;
                }
            }
            catch { }
            return null;
        }
        public async Task<Clues> GetClueFromNFCAsync(string participant,string HFCode)
        {
            try
            {
                string RestUrl = "http://nfcmai.azurewebsites.net/api/TreasureHuntTips?participant={0}&HFCode={1}";
                var uri = new Uri(string.Format(RestUrl,participant,HFCode));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Clues Items = JsonConvert.DeserializeObject<Clues>(content);
                    return Items;
                }
            }
            catch { }
            return null;
        }       
    }
}