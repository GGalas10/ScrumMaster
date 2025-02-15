namespace ScrumMaster.Sprints.Core.Models
{
    public class Sprint
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
