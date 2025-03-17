using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        private readonly string _apiKey;

        public object Id { get; private set; }

        public SitesService()
        {
            // on utilise le fichier de configuration secrets.config pour récupérer les secrets
            var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "secrets.config");

            // on vérifie si le fichier existe
            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException("Le fichier de configuration des secrets est introuvable.", configFilePath);
            }

            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            _httpClient = new HttpClient { BaseAddress = new Uri(config.AppSettings.Settings["ApiBaseUrl"].Value) };
            _apiKey = config.AppSettings.Settings["ApiKey"].Value;
            AddApiKeyHeader();
        }

        private void AddApiKeyHeader()
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            }
        }

        // Récupérer tous les sites
        public async Task<List<Sites>> GetSitesAsync()
        {
            try
            {
                var url = "api/Sites";
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
                var url = $"/api/Sites/get/name/{ville}";
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new List<Salaries>();
                }

                return JsonSerializer.Deserialize<List<Salaries>>(content) ?? new List<Salaries>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des salariés : {ex.Message}", "Erreur");
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
                var url = "/api/Sites";
                var response = await _httpClient.PostAsync(url, content);

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
                var url = $"/api/Sites/{site.Id}";
                var response = await _httpClient.PutAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la mise à jour du site : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // Supprimer un site
        public async Task<bool> DeleteSiteAsync(int id)
        {
            try
            {
                var url = $"/api/Sites/{id}";
                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Le site a été supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(content, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show($"Erreur API : {response.StatusCode}", "Erreur");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception : {ex.Message}", "Erreur");
                return false;
            }
        }
    }
}