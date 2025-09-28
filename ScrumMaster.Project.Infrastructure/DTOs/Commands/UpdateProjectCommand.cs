namespace ScrumMaster.Project.Infrastructure.DTOs.Commands
{
    public class UpdateProjectCommand
    {
        public Guid projectId { get; set; }
        public Guid userId { get; set; }
        public string projectName { get; set; }
        public string projectDescription { get; set; }
    }
}
