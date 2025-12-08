using ScrumMaster.Tasks.Infrastructure.Contracts;
using ScrumMaster.Tasks.Infrastructure.Exceptions;
using System.Text.Json;

namespace ScrumMaster.Tasks.Infrastructure.Implementations
{
    internal class SprintAPIService : ISprintAPIService
    {
        private readonly HttpClient _httpClient;
        public SprintAPIService(IHttpClientFactory httpFactory)
        {
            _httpClient = httpFactory.CreateClient("Sprint");
        }
        public async Task CheckSprintExist(Guid sprintId)
        {
            var result = await _httpClient.GetAsync($"Sprint/CheckSprintExist?sprintId={sprintId}");
            if (result.IsSuccessStatusCode)
            {
                var sprintExist = JsonSerializer.Deserialize<bool>(await result.Content.ReadAsStringAsync());
                if(sprintExist)
                    return;
            }
            throw new BadRequestException("Cannot_Find_Sprint_In_Database");
        }

        public async Task<Guid> GetProjectIdBySprintId(Guid sprintId)
        {
            var result = await _httpClient.GetAsync($"Sprint/GetProjectId?sprintId={sprintId}");
            if (result.IsSuccessStatusCode)
            {
                var projectId = JsonSerializer.Deserialize<Guid>(await result.Content.ReadAsStringAsync());
                if(projectId != Guid.Empty)
                    return projectId;
            }
            throw new BadRequestException("Cannot_Find_Sprint_In_Database");
        }
    }
}
