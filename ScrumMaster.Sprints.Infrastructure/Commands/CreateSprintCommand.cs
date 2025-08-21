using ScrumMaster.Sprints.Core.Models;

namespace ScrumMaster.Sprints.Infrastructure.Commands
{
    public class CreateSprintCommand
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public Guid CreatedUserId { get; set; }
        public Guid ProjectId { get; set; }
        public static Sprint GetModelFromCommand(CreateSprintCommand command)
        {
            return new Sprint(command.Name,command.StartDate,command.EndDate,command.CreatedBy,command.CreatedUserId,command.ProjectId){};
        }
    }
}
