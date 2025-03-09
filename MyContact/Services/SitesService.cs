using System.Net.Http;
using System.Text;
using System.Text.Json;
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
                if (string.IsNullOrWhiteSpace(site.Ville))
                {
                    MessageBox.Show("Erreur : Le site n'a pas de nom.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                var json = JsonSerializer.Serialize(site);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Sites", content);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMsg = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erreur API : {errorMsg}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout : {ex.Message}");
                return false;
            }
        }

        //Modifier un site
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
                MessageBox.Show($"Erreur lors de la mise à jour du site : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        //Supprimer un service
        public async Task<bool> DeleteSiteAsync(int siteId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/Sites/{siteId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression : {ex.Message}");
                return false;
            }
        }

    }
}