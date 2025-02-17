using Microsoft.EntityFrameworkCore;
using ScrumMaster.Sprints.Core.Models;
using ScrumMaster.Sprints.Infrastructure.Commands;
using ScrumMaster.Sprints.Infrastructure.Contracts;
using ScrumMaster.Sprints.Infrastructure.DataAccess;
using ScrumMaster.Sprints.Infrastructure.DTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ScrumMaster.Sprints.Infrastructure.Implementations
{
    public class SprintService : ISprintService
    {
        private readonly ISprintDbContext _context;
        public SprintService(ISprintDbContext sprintDbContext)
        {
            _context = sprintDbContext;
        }
        public async Task<Guid> CreateNewSprintAsync(CreateSprintCommand command)
        {
            if (command == null)
                throw new Exception("Command_Cannot_Be_Null");

            var newSprint = CreateSprintCommand.GetModelFromCommand(command);
            _context.Sprints.Add(newSprint);
            await _context.SaveChangesAsync();
            return newSprint.Id;
        }

        public async Task UpdateSprintAsync(UpdateSprintCommand command)
        {
            if (command == null)
                throw new Exception("Command_Cannot_Be_Null");

            var oldSprint = await _context.Sprints.FirstOrDefaultAsync(x=>x.Id == command.SprintId);
            if (oldSprint == null)
                throw new Exception("Cannot_Find_Sprint_In_Database");

            if (oldSprint.UpdateSprint(UpdateSprintCommand.GetModelFromCommand(command)))
                await _context.SaveChangesAsync();
            else
                throw new Exception("There_are_no_changes_for_sprint");
        }

        public async Task DeleteSprintAsync(Guid id)
        {
            var oldSprint = await _context.Sprints.FirstOrDefaultAsync(x => x.Id == id);
            if (oldSprint == null)
                throw new Exception("Cannot_Find_Sprint_In_Database");

            _context.Sprints.Remove(oldSprint);
            await _context.SaveChangesAsync();
        }

        public async Task<SprintDTO> GetSprintByIdAsync(Guid id)
        => SprintDTO.GetFromModel(await _context.Sprints.FirstOrDefaultAsync(x=>x.Id == id));

        public async Task<List<SprintDTO>> GetAllUserSprintsAsync(Guid userId)
        => await _context.Sprints.Where(x=>x.CreatedUserId == userId).Select(x=> SprintDTO.GetFromModel(x)).ToListAsync();
    }
}
