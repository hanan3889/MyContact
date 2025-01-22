using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyContact.Models;

namespace MyContact.Services
{
    public class SalariesService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public SalariesService()
        {
            _baseUrl = "http://localhost:5110/api/salaries/";
            var handler = new HttpClientHandler();
            _httpClient = new HttpClient(handler) { BaseAddress = new Uri(_baseUrl) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Salaries>> GetSalariesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("get/all");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Salaries>>(content);
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération des salaries.", ex);
            }
        }

        public async Task<List<Salaries>> GetSalariesByNameAsync(string name)
        {
            try
            {
                var response = await _httpClient.GetAsync($"get/name/{name}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Salaries>>(content);
            }
            catch (Exception ex)
            {
                throw new Exception($" Pas de salarié trouvé à ce nom.", ex);
            }
        }
    }
}
