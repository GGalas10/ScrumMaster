using Microsoft.EntityFrameworkCore;
using ScrumMaster.Project.Core.Enums;
using ScrumMaster.Project.Core.Models;
using ScrumMaster.Project.Infrastructure.Contracts;
using ScrumMaster.Project.Infrastructure.CustomExceptions;
using ScrumMaster.Project.Infrastructure.DataAccesses;
using ScrumMaster.Project.Infrastructure.DTOs.Commands;

namespace ScrumMaster.Project.Infrastructure.Implementations
{
    public class AccessService : IAccessService
    {
        public IProjectDbContext _projectDbContext;
        public AccessService(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }
        public async Task AddUserAccess(AccessCommand command)
        {
            if(command == null)
                throw new BadRequestException("Command_Cannot_Be_Null");
            var project = await _projectDbContext.Projects.AsNoTracking().AnyAsync(p => p.Id == command.projectId);
            if (!project)
                throw new NotFoundException("Project_Not_Found");
            var userRole =  await GetUserProjectRole(command.projectId, command.adminId);
            if (userRole != ProjectRoleEnum.Admin)
                throw new RoleException("Only_Admin_Can_Add_User_Access");
            var existingAccess = await _projectDbContext.ProjectUserAccesses.AsNoTracking().FirstOrDefaultAsync(p => p.ProjectId == command.projectId && p.UserId == command.userId);
            if (existingAccess != null)
                throw new RoleException("User_Access_Already_Exists");
            var newAccess = new ProjectUserAccess(command.projectId, command.userId, command.roleEnum);
        }
        public async Task UpdateUserAccess(AccessCommand command)
        {
            if (command == null)
                throw new BadRequestException("Command_Cannot_Be_Null");
            var access = await _projectDbContext.ProjectUserAccesses.FirstOrDefaultAsync(p => p.ProjectId == command.projectId && p.UserId == command.userId);
            if (access == null)
                throw new NotFoundException("Project_Access_Not_Found");
            var adminRole = await GetUserProjectRole(command.projectId, command.adminId);
            if (adminRole != ProjectRoleEnum.Admin)
                throw new RoleException("Only_Admin_Can_Update_User_Access");
            access.SetUserRole(command.roleEnum);
            await _projectDbContext.SaveChangesAsync();
        }
        public async Task RemoveUserAccess(DeleteAccessCommand command)
        {
            if (command == null)
                throw new BadRequestException("Command_Cannot_Be_Null");
            var access = await _projectDbContext.ProjectUserAccesses.FirstOrDefaultAsync(p => p.ProjectId == command.projectId && p.UserId == command.userId);
            if (access == null)
                throw new NotFoundException("Project_Access_Not_Found");
            var adminRole = await GetUserProjectRole(command.projectId, command.adminId);
            if (adminRole != ProjectRoleEnum.Admin)
                throw new RoleException("Only_Admin_Can_Update_User_Access");
            _projectDbContext.ProjectUserAccesses.Remove(access);
            await _projectDbContext.SaveChangesAsync();
        }

        public async Task<ProjectRoleEnum> GetUserProjectRole(Guid projectId, Guid userId)
        {
            if(projectId == Guid.Empty)
                throw new BadRequestException("ProjectId_Cannot_Be_Empty");
            if(userId == Guid.Empty)
                throw new BadRequestException("UserId_Cannot_Be_Empty");
            var projectAccess = await _projectDbContext.ProjectUserAccesses.AsNoTracking().FirstOrDefaultAsync(p => p.ProjectId == projectId && p.UserId == userId);
            if (projectAccess == null)
                throw new NotFoundException("Project_Access_Not_Found");
            return projectAccess.UserRole;
        }
    }
}
