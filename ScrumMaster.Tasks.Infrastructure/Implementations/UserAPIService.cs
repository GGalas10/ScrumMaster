using ScrumMaster.Tasks.Infrastructure.Contracts;
using ScrumMaster.Tasks.Infrastructure.DTOs.Users;
using ScrumMaster.Tasks.Infrastructure.Exceptions;
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
    }
}
