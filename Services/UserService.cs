using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GamingApp.Models;
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
            var url = "https://jsonplaceholder.typicode.com/users";
            var response = await _httpClient.GetStringAsync(url);
            return JsonSerializer.Deserialize<List<User>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}