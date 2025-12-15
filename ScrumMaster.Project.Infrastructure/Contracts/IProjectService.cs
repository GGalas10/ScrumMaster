using ScrumMaster.Project.Infrastructure.DTOs;
using ScrumMaster.Project.Infrastructure.DTOs.Commands;

namespace ScrumMaster.Project.Infrastructure.Contracts
{
    public interface IProjectService
    {
        Task<BoardInfoDTO> GetBoardInfo(Guid projectId,Guid userId);
        Task<Guid> AddNewProject(AddProjectCommand command);
        Task UpdateProject(UpdateProjectCommand command);
        Task<List<UserProjects>> GetUsersProject(Guid userId);
        Task<List<MemberDTO>> GetAllProjectMembers(Guid projectId);
    }
}
