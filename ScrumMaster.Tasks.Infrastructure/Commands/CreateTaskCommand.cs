using ScrumMaster.Tasks.Core.Enums;

namespace ScrumMaster.Tasks.Infrastructure.Commands
{
    public class CreateTaskCommand
    {
        public string title { get; set; }
        public string description { get; set; }
        public Guid sprintId { get; set; }
        public StatusEnum status { get; set; }
        public Guid createdById { get; set; }
    }
}
