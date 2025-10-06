using Microsoft.EntityFrameworkCore;
using ScrumMaster.Project.Core.Enums;
using ScrumMaster.Project.Core.Models;
using ScrumMaster.Project.Infrastructure.Contracts;
using ScrumMaster.Project.Infrastructure.CustomExceptions;
using ScrumMaster.Project.Infrastructure.DataAccesses;
using ScrumMaster.Project.Infrastructure.DTOs;
using ScrumMaster.Project.Infrastructure.DTOs.Commands;

namespace ScrumMaster.Project.Infrastructure.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IAccessService _accessService;
        public ProjectService(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
            _accessService = new AccessService(_projectDbContext);
        }
        public async Task<BoardInfoDTO> GetBoardInfo(Guid projectId, Guid userId)
        {
            if(projectId == Guid.Empty)
                throw new BadRequestException("ProjectId_Cannot_Be_Empty");
            var project = await _projectDbContext.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null)
                throw new NotFoundException("Project_Not_Found");
            var userRole = await _accessService.GetUserProjectRole(projectId, userId);
            if (userRole == ProjectRoleEnum.None)
                throw new RoleException("User_Has_No_Access_To_This_Project");
            var boardInfo = new BoardInfoDTO() { projectDescription = project.ProjectDescription};
            return boardInfo;
        }      
        public async Task<Guid> AddNewProject(AddProjectCommand command)
        {
            if(command == null)
                throw new BadRequestException("Command_Cannot_Be_Null");
            var newProject = new ProjectModel(command.projectName, command.projectDescription);
            _projectDbContext.Projects.Add(newProject);
            await _projectDbContext.SaveChangesAsync();
            await _accessService.AddUserAccess(new AccessCommand
            {
                adminId = command.userId,
                projectId = newProject.Id,
                roleEnum = ProjectRoleEnum.Admin,
                userId = command.userId
            });
            return newProject.Id;
        }
        public async Task UpdateProject(UpdateProjectCommand command)
        {
            if (command == null)
                throw new BadRequestException("Command_Cannot_Be_Null");
            var project = await _projectDbContext.Projects.FirstOrDefaultAsync(p => p.Id == command.projectId);
            if (project == null)
                throw new NotFoundException("Project_Not_Found");
            var userRole = await _accessService.GetUserProjectRole(command.projectId, command.userId);
            if(userRole != ProjectRoleEnum.Admin || userRole != ProjectRoleEnum.Member)
                throw new RoleException("Only_Admin_And_Member_Can_Update_Project");
            try
            {
                project.UpdateProject(command.projectName, command.projectDescription);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }
            _projectDbContext.Projects.Update(project);
            await _projectDbContext.SaveChangesAsync();
        }
        public async Task<List<UserProjects>> GetUsersProject(Guid userId)
        {
            var userProjects = await _projectDbContext.ProjectUserAccesses.Include(x=>x.Project).AsNoTracking().Where(p=>p.UserId == userId)
                .Select(p => new UserProjects
                {
                    projectId = p.ProjectId,
                    projectName = p.Project.ProjectName,
                    userRole = p.UserRole
                }).ToListAsync();
            return userProjects;
        }
    }
}
