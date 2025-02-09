using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
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
            _httpClient = new HttpClient();
        }

        //public async Task<List<Salaries>> GetSalariesByCityAsync(string ville)
        //{
        //    try
        //    {

        //        var response = await _httpClient.GetAsync($"https://localhost:7140/api/Sites/get/name/{ville}");

        //        if (!response.IsSuccessStatusCode)
        //        {

        //            return new List<Salaries>(); 
        //        }

        //        var content = await response.Content.ReadAsStringAsync();
        //        var salaries = JsonSerializer.Deserialize<List<Salaries>>(content);


        //        return salaries ?? new List<Salaries>();
        //    }
        //    catch (Exception ex)
        //    {

        //        return new List<Salaries>();
        //    }
        //}
        public async Task<List<Salaries>> GetSalariesByCityAsync(string ville)
        {
            try
            {
                var url = $"https://localhost:7140/api/Sites/get/name/{ville}";
                Debug.WriteLine($"🔎 Requête API vers : {url}");

                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"📥 Réponse API (brute) : {content}"); // Ajoute ce log

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"❌ Erreur API : {response.StatusCode}");
                    return new List<Salaries>();
                }

                var salaries = JsonSerializer.Deserialize<List<Salaries>>(content);

                Debug.WriteLine($"✅ {salaries?.Count ?? 0} salariés trouvés !");
                return salaries ?? new List<Salaries>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Exception : {ex.Message}");
                return new List<Salaries>();
            }
        }

    }
}
