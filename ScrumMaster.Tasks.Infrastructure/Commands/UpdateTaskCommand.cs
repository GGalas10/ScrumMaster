using ScrumMaster.Tasks.Core.Enums;

namespace ScrumMaster.Tasks.Infrastructure.Commands
{
    public class UpdateTaskCommand
    {
        public string title { get; set; }
        public string description { get; set; }
        public StatusEnum status { get; set; }
        public Guid assignedUserId { get; set; }
        public Guid sprintId { get; set; }
    }
}
