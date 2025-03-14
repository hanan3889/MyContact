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
    public class SalariesService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public SalariesService()
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

        public async Task<List<Salaries>> GetSalariesAsync()
        {
            var url = "api/Salaries/get/all";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Salaries>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<Salaries>();
        }

        public async Task<bool> CreateSalaryAsync(Salaries salary)
        {
            var json = JsonSerializer.Serialize(salary, new JsonSerializerOptions { WriteIndented = true });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "api/Salaries/create";
            var response = await _httpClient.PostAsync(url, content);
            string responseText = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSalaryAsync(Salaries salary)
        {
            var json = JsonSerializer.Serialize(salary);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"api/Salaries/update/{salary.Id}";
            var response = await _httpClient.PutAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSalaryAsync(int id)
        {
            var url = $"api/Salaries/delete/{id}";
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Salaries>> GetSalariesByNameAsync(string name)
        {
            var url = $"api/Salaries/get/name/{name}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Salaries>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<Salaries>();
        }

        public async Task<List<ServicesModel>> GetServicesAsync()
        {
            var url = "api/Services";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ServicesModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ServicesModel>();
            }
            return new List<ServicesModel>();
        }

        public async Task<List<Sites>> GetSitesAsync()
        {
            var url = "api/Sites";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Sites>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Sites>();
            }
            return new List<Sites>();
        }
    }
}