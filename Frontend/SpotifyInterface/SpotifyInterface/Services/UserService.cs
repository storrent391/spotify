using System.Net.Http;
using System.Text;
using System.Text.Json;
using SpotifyInterface.Models;

namespace SpotifyInterface.Services;

public class UserService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:5000";

    public UserService()
    {
        _httpClient = new HttpClient();
    }

    // GET /users
    public async Task<List<User>> GetAllAsync()
    {
        string url = $"{_baseUrl}/users";
        
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            List<User>? users = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });
            
            return users ?? new List<User>();
        }
        
        return new List<User>();
    }

    // GET /user/{id} 
    public async Task<User?> GetByIdAsync(Guid id)
    {
        string url = $"{_baseUrl}/user/{id}";
        
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            User? user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });
            
            return user;
        }
        
        return null;
    }
}