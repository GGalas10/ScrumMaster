namespace ScrumMaster.Project.Infrastructure.DTOs.Commands
{
    public class DeleteAccessCommand
    {
        public Guid userId { get; set; }
        public Guid adminId { get; set; }
        public Guid projectId { get; set; }
    }
}
