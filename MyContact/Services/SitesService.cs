using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
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

        // Récupération de tous les sites
        //public async Task<List<Sites>> GetSitesAsync()
        //{
        //    try
        //    {
        //        //var response = await _httpClient.GetAsync("https://localhost:7140/api/Sites");
        //        //var content = await response.Content.ReadAsStringAsync();

        //        var url = "https://localhost:7140/api/Sites";
        //        var response = await _httpClient.GetAsync(url);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            MessageBox.Show($"Erreur API : {response.StatusCode}", "Erreur");
        //            return new List<Sites>();
        //        }
        //        var content = await response.Content.ReadAsStringAsync();

        //        return JsonSerializer.Deserialize<List<Sites>>(content) ?? new List<Sites>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Erreur lors de la récupération des sites : {ex.Message}");
        //        return new List<Sites>();
        //    }
        //}

        // Récupérer tous les services
        public async Task<List<Sites>> GetSitesAsync()
        {
            try
            {
                var url = "https://localhost:7140/api/Sites";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Erreur API : {response.StatusCode}", "Erreur");
                    return new List<Sites>();
                }

                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var sites = JsonSerializer.Deserialize<List<Sites>>(content, options) ?? new List<Sites>();

                return sites;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception : {ex.Message}", "Erreur");
                return new List<Sites>();
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
        public async Task<bool> AddSiteAsync(Sites site)
        {
            try
            {
                var json = JsonSerializer.Serialize(site);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/Sites", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout du site : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // Mise à jour d'un site
        public async Task<bool> UpdateSiteAsync(Sites site)
        {
            try
            {
                var json = JsonSerializer.Serialize(site);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Sites/{site.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la mise à jour du site : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        internal async Task<bool> DeleteSiteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
