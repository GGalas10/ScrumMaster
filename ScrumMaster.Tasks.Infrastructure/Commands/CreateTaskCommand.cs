namespace ScrumMaster.Tasks.Infrastructure.Commands
{
    public class CreateTaskCommand
    {
        public string title { get; set; }
        public string description { get; set; }
        public Guid sprintId { get; set; }
    }
}
