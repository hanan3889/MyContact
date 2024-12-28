using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
            _baseUrl = "https://localhost:5110/api/salaries";
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
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
                throw new Exception("Erreur lors de la récupération des salaires.", ex);
            }
        }

        public async Task<Salaries> GetSalaryByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"get/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Salaries>(content);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération du salaire avec l'ID {id}.", ex);
            }
        }

        public async Task CreateSalaryAsync(Salaries salary)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(salary), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("create", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la création du salaire.", ex);
            }
        }

        public async Task UpdateSalaryAsync(int id, Salaries salary)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(salary), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"update/{id}", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la mise à jour du salaire avec l'ID {id}.", ex);
            }
        }

        public async Task DeleteSalaryAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"delete/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la suppression du salaire avec l'ID {id}.", ex);
            }
        }
    }
}
