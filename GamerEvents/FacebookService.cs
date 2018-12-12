﻿using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Xamarin_FacebookAuth.Services
{
    public class FacebookService
    {
        public async Task<string> GetEmailAsync(string accessToken)
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email&access_token={accessToken}");
            var email = JsonConvert.DeserializeObject<FacebookEmail>(json);
            return email.Email;
        }

        public async Task<string> GetNameAsync(string accessToken)
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=name&access_token={accessToken}");
            var name = JsonConvert.DeserializeObject<FacebookName>(json);
            return name.Name;
        }
    }

    public class FacebookEmail
    {
        public string Email { get; set; }
    }

    public class FacebookName
    {
        public string Name { get; set; }
    }
}