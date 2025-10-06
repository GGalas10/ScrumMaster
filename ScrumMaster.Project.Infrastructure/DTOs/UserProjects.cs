using ScrumMaster.Project.Core.Enums;

namespace ScrumMaster.Project.Infrastructure.DTOs
{
    public class UserProjects
    {
        public Guid projectId { get; set; }
        public string projectName { get; set; }
        public ProjectRoleEnum userRole { get; set; }
    }
}
