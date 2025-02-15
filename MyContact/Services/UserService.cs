using System.Net.Http;
using System.Text;
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


        public async Task<bool> AuthenticateUser(string email, string password)
        {
            var user = new { Email = email, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/api/Users/login", content);

            return response.IsSuccessStatusCode;
        }
    }
}
