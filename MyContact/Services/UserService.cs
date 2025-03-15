using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MyContact.Models;
using Newtonsoft.Json;
using BCrypt.Net;

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

        public async Task<bool> RegisterUser(string email, string password, string secretCode)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new { Email = email, Password = hashedPassword, SecretCode = secretCode, Roles = 1 };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/api/Users/register", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<Users?> AuthenticateUser(string email, string password, string secretCode)
        {
            var userCredentials = new { Email = email, Password = password, SecretCode = secretCode };
            var content = new StringContent(JsonConvert.SerializeObject(userCredentials), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/api/Users/login", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                return null;
            }

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Users>(json);
        }

        public async Task<Users?> GetUserByEmail(string email)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Users/email/{email}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Users>(json);
        }
    }
}