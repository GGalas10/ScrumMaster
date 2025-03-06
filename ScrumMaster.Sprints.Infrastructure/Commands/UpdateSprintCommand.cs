namespace ScrumMaster.Sprints.Infrastructure.Commands
{
    public class UpdateSprintCommand
    {
        public Guid SprintId { get; set; }
        public string SprintName { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string CreateBy { get; set; }
    }
}
