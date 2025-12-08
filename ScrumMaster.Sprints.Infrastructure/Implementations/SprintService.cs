using Microsoft.EntityFrameworkCore;
using ScrumMaster.Sprints.Core.Enums;
using ScrumMaster.Sprints.Infrastructure.Commands;
using ScrumMaster.Sprints.Infrastructure.Contracts;
using ScrumMaster.Sprints.Infrastructure.DataAccess;
using ScrumMaster.Sprints.Infrastructure.DTO;
using ScrumMaster.Sprints.Infrastructure.Exceptions;
using System.Text.Json;

namespace ScrumMaster.Sprints.Infrastructure.Implementations
{
    public class SprintService : ISprintService
    {
        private readonly ISprintDbContext _context;
        private readonly HttpClient _identityHttpClient;
        public SprintService(ISprintDbContext sprintDbContext, IHttpClientFactory httpFactory)
        {
            _context = sprintDbContext;
            _identityHttpClient = httpFactory.CreateClient("Project");
        }
        public async Task<Guid> CreateNewSprintAsync(CreateSprintCommand command, Guid userId, string userFullName)
        {
            if(!await UserHavePremissions(userId, command.ProjectId, UserPremissionsEnum.CanSave))
                throw new BadRequestException("User_Doesnt_Have_Premissions");

            if (command == null)
                throw new BadRequestException("Command_Cannot_Be_Null");

            command.CreatedUserId = userId;
            command.CreatedBy = userFullName;
            var newSprint = CreateSprintCommand.GetModelFromCommand(command);
            if (!await _context.Sprints.AnyAsync(x => x.ProjectId == command.ProjectId))
                newSprint.SetActual(true);
            _context.Sprints.Add(newSprint);
            await _context.SaveChangesAsync();
            return newSprint.Id;
        }
        public async Task UpdateSprintAsync(UpdateSprintCommand command,Guid userId)
        {
            
            if (command == null)
                throw new BadRequestException("Command_Cannot_Be_Null");

            var oldSprint = await _context.Sprints.FirstOrDefaultAsync(x => x.Id == command.SprintId);
            if (oldSprint == null)
                throw new BadRequestException("Cannot_Find_Sprint_In_Database");

            if (!await UserHavePremissions(userId, oldSprint.ProjectId, UserPremissionsEnum.CanSave))
                throw new BadRequestException("User_Doesnt_Have_Premissions");

            if (oldSprint.UpdateSprint(command.SprintName, command.CreateBy, command.StartAt, command.EndAt))
                await _context.SaveChangesAsync();
            else
                throw new BadRequestException("There_are_no_changes_for_sprint");
        }
        public async Task DeleteSprintAsync(Guid id, Guid userId)
        {          
            var oldSprint = await _context.Sprints.FirstOrDefaultAsync(x => x.Id == id);

            if (oldSprint == null)
                throw new BadRequestException("Cannot_Find_Sprint_In_Database");

            if (!await UserHavePremissions(userId, oldSprint.ProjectId, UserPremissionsEnum.CanDelete))
                throw new BadRequestException("User_Doesnt_Have_Premissions");

            _context.Sprints.Remove(oldSprint);
            await _context.SaveChangesAsync();
        }
        public async Task<SprintDTO> GetSprintByIdAsync(Guid id)
        => SprintDTO.GetFromModel(await _context.Sprints.FirstOrDefaultAsync(x => x.Id == id));
        public async Task<List<SprintDTO>> GetAllUserSprintsAsync(Guid userId)
        => await _context.Sprints.Where(x => x.CreatedUserId == userId).Select(x => SprintDTO.GetFromModel(x)).ToListAsync();
        public async Task<bool> CheckSprintExist(Guid sprintId)
            => await _context.Sprints.AnyAsync(x => x.Id == sprintId);
        public async Task<List<SprintDTO>> GetSprintsByProjectId(Guid projectId, Guid userId)
        {
            if (!await UserHavePremissions(userId, projectId, UserPremissionsEnum.CanRead))
                throw new BadRequestException("User_Doesnt_Have_Premissions");

            if (projectId == Guid.Empty)
                throw new BadRequestException("ProjectId_Cannot_Be_Empty");

            return await _context.Sprints
                .Where(x => x.ProjectId == projectId)
                .Select(x => SprintDTO.GetFromModel(x))
                .ToListAsync();
        }
        public async Task<Guid> GetActualSprintByProjectId(Guid projectId, Guid userId)
        {

            if (projectId == Guid.Empty)
                throw new BadRequestException("ProjectId_Cannot_Be_Empty");

            if(!await _context.Sprints.AnyAsync(x=>x.ProjectId == projectId))
                throw new BadRequestException("Project_Doesnt_Have_Sprint");

            var result = await _context.Sprints.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.IsActual);
            if (result == null)
                return await _context.Sprints.Where(x => x.ProjectId == projectId).Select(x=>x.Id).FirstOrDefaultAsync();
            return result.Id;
        }
        public async Task<Guid> GetProjectIdBySprintId(Guid sprintId)
        {
            var sprint = await _context.Sprints.FirstOrDefaultAsync(x => x.Id == sprintId);
            if (sprint == null)
                throw new BadRequestException("Cannot_Find_Sprint_In_Database");
            return sprint.ProjectId;
        }
        private async Task<bool> UserHavePremissions(Guid userId, Guid projectId, UserPremissionsEnum role)
        {
            var response = await _identityHttpClient.GetAsync($"GetUserProjectRole?projectId={projectId}&userId={userId}");
            response.EnsureSuccessStatusCode();
            var members = JsonSerializer.Deserialize<ProjectRoleEnum>(await response.Content.ReadAsStringAsync());
            switch(role)
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
            return false;
        }
    }
}
