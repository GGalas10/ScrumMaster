namespace ScrumMaster.Project.Infrastructure.DTOs.Commands
{
    public class AddProjectCommand
    {
        public Guid userId { get; set; }
        public string projectName { get; set; }
        public string projectDescription { get; set; }
    }
}
