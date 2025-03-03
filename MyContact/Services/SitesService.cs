using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyContact.Models;

namespace MyContact.Services
{
    public class SitesService
    {
        private readonly HttpClient _httpClient;

        public SitesService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5110") };
        }

        // Mise à jour d'un site
        public async Task<bool> UpdateSiteAsync(Sites site)
        {
            try
            {
                string json = JsonSerializer.Serialize(site);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/api/Sites/{site.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour du site : {ex.Message}");
                return false;
            }
        }

        // Récupération des salariés par ville
        public async Task<List<Salaries>> GetSalariesByCityAsync(string ville)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/Sites/get/name/{ville}");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new List<Salaries>();
                }

                return JsonSerializer.Deserialize<List<Salaries>>(content) ?? new List<Salaries>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des salariés : {ex.Message}");
                return new List<Salaries>();
            }
        }

        // Ajout d'un site
        public async Task<bool> AddSiteAsync(Sites newSite)
        {
            try
            {
                string json = JsonSerializer.Serialize(newSite);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Sites", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout du site : {ex.Message}");
                return false;
            }
        }
    }
}
