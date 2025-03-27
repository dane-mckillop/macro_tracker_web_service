using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using macro_tracker_web_service.Models;

namespace macro_tracker_web_service.Services
{
    public class FoodService
    {
        private readonly HttpClient _httpClient;
        private const string CoreServiceBaseUrl = "http://localhost:5105/api/Foods"; // Update with your core service URL

        public FoodService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<FoodPer100g>> GetAllFoodsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<FoodPer100g>>(CoreServiceBaseUrl);
                return response ?? new List<FoodPer100g>();
            }
            catch (HttpRequestException ex)
            {
                throw new DbUpdateException("Failed to retrieve foods from core service", ex);
            }
        }

        public async Task<FoodPer100g?> GetFoodByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{CoreServiceBaseUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FoodPer100g>();
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();
                return null;
            }
            catch (HttpRequestException ex)
            {
                throw new ArgumentException($"Error fetching food with ID {id}", ex);
            }
        }

        public async Task<FoodPer100g?> GetFoodByNameAsync(string name)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{CoreServiceBaseUrl}/name/{name}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FoodPer100g>();
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();
                return null;
            }
            catch (HttpRequestException ex)
            {
                throw new ArgumentException($"Error fetching food with name {name}", ex);
            }
        }

        public async Task<List<FoodPer100g>> GetFoodBySubstringAsync(string substring)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{CoreServiceBaseUrl}/substring/{substring}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<FoodPer100g>>()
                           ?? new List<FoodPer100g>();
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<FoodPer100g>();
                }

                response.EnsureSuccessStatusCode();
                return new List<FoodPer100g>();
            }
            catch (HttpRequestException ex)
            {
                throw new ArgumentException($"Error fetching foods with substring {substring}", ex);
            }
        }

        public async Task<FoodPer100g?> CreateFoodAsync(FoodPer100g food)
        {
            if (food == null)
            {
                throw new ArgumentException("Food item cannot be null");
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync(CoreServiceBaseUrl, food);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FoodPer100g>();
                }

                response.EnsureSuccessStatusCode();
                return null;
            }
            catch (HttpRequestException ex)
            {
                throw new DbUpdateException("Error creating food item", ex);
            }
        }

        public async Task UpdateFoodAsync(FoodPer100g food)
        {
            if (food == null)
            {
                throw new ArgumentException("Food item cannot be null");
            }

            try
            {
                var response = await _httpClient.PutAsJsonAsync(CoreServiceBaseUrl, food);

                if (!response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (HttpRequestException ex)
            {
                throw new DbUpdateException($"Error updating food with ID {food.FoodId}", ex);
            }
        }
    }
}