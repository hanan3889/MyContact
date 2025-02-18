using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyContact.Models;

namespace MyContact.Services
{
    public class SalariesService
    {
        private readonly HttpClient _httpClient;

        public SalariesService()
        {
            _httpClient = new HttpClient { BaseAddress = new System.Uri("https://localhost:7140/api/Salaries/") };
        }

        public async Task<List<Salaries>> GetSalariesAsync()
        {
            var response = await _httpClient.GetAsync("get/all");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Salaries>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<Salaries>();
        }

        public async Task<bool> CreateSalaryAsync(Salaries salary)
        {
            var json = JsonSerializer.Serialize(salary);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("create", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSalaryAsync(Salaries salary)
        {
            var json = JsonSerializer.Serialize(salary);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"update/{salary.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSalaryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"delete/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Salaries>> GetSalariesByNameAsync(string name)
        {
            var response = await _httpClient.GetAsync($"get/name/{name}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Salaries>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<Salaries>();
        }

    }
}
