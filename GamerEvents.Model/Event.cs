using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace GamerEvents.DBModel
{
    public class Event
    {
        private static WebClient _webClient = new WebClient();

        public int eventid { get; set; }
        public int ownerid { get; set; }
        public DateTime date { get; set; }
        public string location { get; set; }
        public string game { get; set; }
        public string details { get; set; }
        public int userlimit { get; set; }



        public static bool CreateNewEvent(Event formEvent)
        {
            string jsonString = JsonConvert.SerializeObject(formEvent);
            string url = "https://pte-ttk.wscdev.hu/team4/event/ecreate.php";
            _webClient.Headers.Add("Content-Type", "application/json");
            string reply = _webClient.UploadString(url, jsonString);

            return JsonConvert.DeserializeObject<bool>(reply);
        }
    }
}
