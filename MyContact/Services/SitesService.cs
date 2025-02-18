﻿using System.Net.Http;
using System.Text.Json;
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



        public async Task<List<Salaries>> GetSalariesByCityAsync(string ville)
        {
            try
            {
                var url = $"http://localhost:5110/api/Sites/get/name/{ville}";
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

    }
}
