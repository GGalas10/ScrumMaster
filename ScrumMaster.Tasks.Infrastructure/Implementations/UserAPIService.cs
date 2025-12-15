using ScrumMaster.Tasks.Infrastructure.Contracts;
using ScrumMaster.Tasks.Infrastructure.DTOs.Users;
using ScrumMaster.Tasks.Infrastructure.Exceptions;
using System.Net.Http.Json;
using System.Text.Json;

namespace ScrumMaster.Tasks.Infrastructure.Implementations
{
    internal class UserAPIService : IUserAPIService
    {
        private readonly HttpClient _httpClient;
        public UserAPIService(IHttpClientFactory httpFactory)
        {
            _httpClient = httpFactory.CreateClient("Identity");
        }
        public async Task<UserDTO> GetUserById(Guid userId)
        {
            var result = await _httpClient.GetAsync($"api/User/GetById?userId={userId}");
            if (result.IsSuccessStatusCode)
            {
                var user = JsonSerializer.Deserialize<UserDTO>(await result.Content.ReadAsStringAsync());
                if(user != null)
                    return user;
            }
            throw new BadRequestException("User_Doesnt_Exist");
        }
        public async Task<List<UserDTO>> GetUsersByIds(List<Guid> userIds)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/User/GetUserByIdsList",userIds);
            if (result.IsSuccessStatusCode)
            {
                var users = JsonSerializer.Deserialize<List<UserDTO>>(await result.Content.ReadAsStringAsync());
                if (users != null)
                    return users;
            }
            throw new BadRequestException("Users_Doesnt_Exist");
        }
    }
}
