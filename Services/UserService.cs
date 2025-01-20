using GamingApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GamingApp.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var url = "https://jsonplaceholder.typicode.com/users";
                var response = await _httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<List<User>>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return new List<User>();
            }
        }
    }
}
