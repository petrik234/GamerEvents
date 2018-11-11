using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace GamerEvents.DBModel
{

    public class User
    {
        private static WebClient _webClient = new WebClient();

        public int userid { get; set; }
        public string email { get; set; }
        public string gplus { get; set; }
        public string password { get; set; }
        public string realname { get; set; }
        public int? age { get; set; }
        public string city { get; set; }
        public string konfig { get; set; }

        public static User GetByEmail(string email)
        {
            string url = "https://pte-ttk.wscdev.hu/team4/user/getByEmail.php?email=" + email;
            _webClient.Headers.Add("Content-Type", "application/json");
            string reply = _webClient.DownloadString(url);

            try
            {
                return JsonConvert.DeserializeObject<User>(reply);
            }
            catch 
            {
                //nincs ilyen rekord
                return null;
            }
        }

        public static User GetById(int id)
        {
            string url = "http://pte-ttk.wscdev.hu/team4/user/getById.php?userid=" + id.ToString();
            _webClient.Headers.Add("Content-Type", "application/json");
            string reply = _webClient.DownloadString(url);

            try
            {
                return JsonConvert.DeserializeObject<User>(reply);
            }
            catch
            {
                //nincs ilyen rekord
                return new User();
            }
        }
        


        public static bool CreateNew(User user)
        {
            string jsonString = JsonConvert.SerializeObject(user);
            string url = "https://pte-ttk.wscdev.hu/team4/user/createByEmail.php";
            _webClient.Headers.Add("Content-Type", "application/json");
            string reply = _webClient.UploadString(url, jsonString);

            return JsonConvert.DeserializeObject<bool>(reply);
        }

        public static bool UpdateById(User user)
        {
            string jsonString = JsonConvert.SerializeObject(user);
            string url = "https://pte-ttk.wscdev.hu/team4/user/updateById.php";
            _webClient.Headers.Add("Content-Type", "application/json");
            string reply = _webClient.UploadString(url, jsonString);

            return JsonConvert.DeserializeObject<bool>(reply);
        }


        

    }
}