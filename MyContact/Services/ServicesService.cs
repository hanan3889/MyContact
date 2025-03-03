using System.Net.Http;
using System.Text.Json;
using System.Windows;
using MyContact.Models;

namespace MyContact.Services
{
    public class ServicesService
    {
        private readonly HttpClient _httpClient;

        public ServicesService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Salaries>> GetSalariesByServiceNameAsync(string serviceName)
        {
            try
            {
                var url = $"http://localhost:5110/api/Services/get/name/{serviceName}";
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

                return new List<Salaries>();
            }
        }

        // Récupérer tous les services
        public async Task<List<ServicesModel>> GetAllServicesAsync()
        {
            try
            {
                var url = "https://localhost:7140/api/Services";
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
                MessageBox.Show($"❌ Exception : {ex.Message}", "Erreur");
                return new List<ServicesModel>();
            }
        }






    }
}
