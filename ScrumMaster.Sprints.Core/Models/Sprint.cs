namespace ScrumMaster.Sprints.Core.Models
{
    public class Sprint
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public Guid CreatedUserId { get; set; }
        public bool UpdateSprint(Sprint sprint)
        {
            var AnyChange = false;
            if (!string.IsNullOrWhiteSpace(sprint.Name) && Name != sprint.Name)
            {
                Name = sprint.Name;
                AnyChange = true;
            }
            if(!string.IsNullOrWhiteSpace(sprint.CreatedBy) && CreatedBy != sprint.CreatedBy)
            {
                CreatedBy = sprint.CreatedBy;
                AnyChange = true;
            }
            if (sprint.StartDate != StartDate)
            {
                StartDate = sprint.StartDate;
                AnyChange = true;
            }
            if (sprint.EndDate != EndDate)
            {
                EndDate = sprint.EndDate;
                AnyChange = true;
            }           
            return AnyChange;            
        }
    }
}
