namespace ScrumMaster.Sprints.Infrastructure.Commands
{
    public class CreateSprintCommand
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
