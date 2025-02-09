﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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
                var url = $"http://localhost:5110/api/Services/get/salaries/{serviceName}";
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
