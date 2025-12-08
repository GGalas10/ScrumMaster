using ScrumMaster.Tasks.Core.Enums;
using ScrumMaster.Tasks.Infrastructure.Contracts;
using ScrumMaster.Tasks.Infrastructure.Exceptions;
using System.Text.Json;

namespace ScrumMaster.Tasks.Infrastructure.Implementations
{
    internal class ProjectAPIService : IProjectAPIService
    {
        private readonly HttpClient _httpClient;
        public ProjectAPIService(IHttpClientFactory httpFactory)
        {
            _httpClient = httpFactory.CreateClient("Project");
        }

        public async Task<bool> UserHavePremissions(Guid userId, Guid projectId, UserPremissionsEnum role)
        {
            var response = await _httpClient.GetAsync($"GetUserProjectRole?projectId={projectId}&userId={userId}");
            response.EnsureSuccessStatusCode();
            var members = JsonSerializer.Deserialize<ProjectRoleEnum>(await response.Content.ReadAsStringAsync());
            switch (role)
            {
                case UserPremissionsEnum.CanRead:
                    if (members == ProjectRoleEnum.Owner || members == ProjectRoleEnum.Admin || members == ProjectRoleEnum.Member || members == ProjectRoleEnum.Guest || members == ProjectRoleEnum.Observer)
                        return true;
                    break;
                case UserPremissionsEnum.CanSave:
                    if (members == ProjectRoleEnum.Owner || members == ProjectRoleEnum.Admin || members == ProjectRoleEnum.Member)
                        return true;
                    break;
                case UserPremissionsEnum.CanDelete:
                    if (members == ProjectRoleEnum.Owner || members == ProjectRoleEnum.Admin || members == ProjectRoleEnum.Member)
                        return true;
                    break;
            }
            throw new BadRequestException("User_Dosent_Have_Premissions");
        }
    }
}
