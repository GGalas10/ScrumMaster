using ScrumMaster.Sprints.Core.Models;

namespace ScrumMaster.Sprints.Infrastructure.Commands
{
    public class UpdateSprintCommand
    {
        public Guid SprintId { get; set; }
        public string SprintName { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string CreateBy { get; set; }
        public static Sprint GetModelFromCommand(UpdateSprintCommand command)
        {
            return new Sprint()
            {
                Id = Guid.NewGuid(),
                Name = command.SprintName,
                StartDate = command.StartAt,
                EndDate = command.EndAt,
                CreatedBy = command.CreateBy
            };
        }
    }
}
