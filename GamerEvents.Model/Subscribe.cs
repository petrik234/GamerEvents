using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GamerEvents.DBModel
{
    public class Subscribe
    {
        private static WebClient _webClient = new WebClient();

        public int subid { get; set; }
        public int userid { get; set; }
        public int eventid { get; set; }

        public static Subscribe[] GetAll()
        {
            string url = "https://pte-ttk.wscdev.hu/team4/subscribe/getAll.php";
            _webClient.Headers.Add("Content-Type", "application/json");
            string reply = _webClient.DownloadString(url);

            try
            {
                return JsonConvert.DeserializeObject<Subscribe[]>(reply); ;
            }
            catch
            {
                return null;
            }
        }


        public static bool CreateNew(Subscribe subscribe)
        {
            string jsonString = JsonConvert.SerializeObject(subscribe);
            string url = "https://pte-ttk.wscdev.hu/team4/subscribe/create.php";
            _webClient.Headers.Add("Content-Type", "application/json");
            string reply = _webClient.UploadString(url, jsonString);

            return JsonConvert.DeserializeObject<bool>(reply);
        }

        public static bool deleteById(int id)
        {
            string jsonString = JsonConvert.SerializeObject(new Subscribe() {subid = id }) ;
            string url = "https://pte-ttk.wscdev.hu/team4/subscribe/deleteById.php";
            _webClient.Headers.Add("Content-Type", "application/json");
            string reply = _webClient.UploadString(url, jsonString);

            return JsonConvert.DeserializeObject<bool>(reply);
        }

    }
}
