using System.Net.Http;
using System.Text;
using System.Windows;
using MyContact.Models;
using Newtonsoft.Json;

namespace MyContact.Services
{
    public class UsersService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public UsersService(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            var user = new { Email = email, Password = password, Roles = 1 };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/api/Users/register", content);

            return response.IsSuccessStatusCode;
        }


        public async Task<Users?> AuthenticateUser(string email, string password)
        {
            var userCredentials = new { Email = email, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(userCredentials), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/api/Users/login", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Erreur API : {response.StatusCode} - {errorContent}", "Erreur Authentification");
                return null;
            }

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Users>(json);
        }
    }
}
