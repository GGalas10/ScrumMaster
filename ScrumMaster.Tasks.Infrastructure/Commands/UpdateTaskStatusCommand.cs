using ScrumMaster.Tasks.Core.Enums;

namespace ScrumMaster.Tasks.Infrastructure.Commands
{
    public class UpdateTaskStatusCommand
    {
        public Guid taskId { get; set; }
        public StatusEnum status { get; set; }
    }
}
