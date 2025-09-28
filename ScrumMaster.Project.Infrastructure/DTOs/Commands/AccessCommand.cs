using ScrumMaster.Project.Core.Enums;

namespace ScrumMaster.Project.Infrastructure.DTOs.Commands
{
    public class AccessCommand
    {
        public Guid userId { get; set; }
        public Guid adminId { get; set; }
        public Guid projectId { get; set; }
        public ProjectRoleEnum roleEnum { get; set; }
    }
}
