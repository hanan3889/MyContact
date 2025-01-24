using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MyContact.Models;

namespace MyContact.Services
{
    public class SitesService
        //Permet d appeler l'API
    {
        private readonly HttpClient _httpClient;

        public SitesService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Salaries>> GetSalariesByCityAsync(string ville)
        {
            var response = await _httpClient.GetAsync($"https://localhost:5001/api/Sites/get/name/{ville}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var salaries = JsonSerializer.Deserialize<List<Salaries>>(content);

            return salaries;
        }
    }
}
