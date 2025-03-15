using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using MyContact.Models;

namespace MyContact.Services
{
    public class ServicesService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public Uri BaseAddress { get; }

        public ServicesService()
        {
            // on utilise le fichier de configuration secrets.config pour récupérer les secrets
            var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "secrets.config");

            // Vérifiez si le fichier existe
            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException("Le fichier de configuration des secrets est introuvable.", configFilePath);
            }

            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            _httpClient = new HttpClient();
            BaseAddress = new Uri(config.AppSettings.Settings["ApiBaseUrl"].Value);
            _httpClient.BaseAddress = BaseAddress;
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

        public async Task<List<Salaries>> GetSalariesByServiceNameAsync(string serviceName)
        {
            try
            {
                var url = $"api/Services/get/name/{serviceName}";
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new List<Salaries>();
                }

                var salaries = JsonSerializer.Deserialize<List<Salaries>>(content);

                return salaries ?? new List<Salaries>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception : {ex.Message}", "Erreur");
                return new List<Salaries>();
            }
        }

        public async Task<List<ServicesModel>> GetAllServicesAsync()
        {
            try
            {
                var url = "api/Services";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Erreur API : {response.StatusCode}", "Erreur");
                    return new List<ServicesModel>();
                }

                var content = await response.Content.ReadAsStringAsync();

                var services = JsonSerializer.Deserialize<List<ServicesModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (services == null)
                {
                    return new List<ServicesModel>();
                }

                return services;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception : {ex.Message}", "Erreur");
                return new List<ServicesModel>();
            }
        }

        public async Task<bool> AddServiceAsync(ServicesModel newService)
        {
            try
            {
                var url = "api/Services";
                var response = await _httpClient.PostAsJsonAsync(url, newService);

                if (response.IsSuccessStatusCode)
                {
                    return true;
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

        public async Task<bool> DeleteServiceAsync(int serviceId)
        {
            try
            {
                var url = $"api/Services/{serviceId}";
                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return true;
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

        public async Task<bool> UpdateServiceAsync(ServicesModel updatedService)
        {
            try
            {
                var url = $"api/Services/{updatedService.Id}";
                var response = await _httpClient.PutAsJsonAsync(url, updatedService);

                if (response.IsSuccessStatusCode)
                {
                    return true;
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