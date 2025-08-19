using ScrumMaster.Project.Core.Enums;

namespace ScrumMaster.Project.Core.Models
{
    public class ProjectUserAccess
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public Guid UserId { get; private set; }
        public ProjectRoleEnum UserRole { get; private set; }
        private ProjectUserAccess() { }
        public ProjectUserAccess(Guid projectId, Guid userId, ProjectRoleEnum userRole)
        {
            Id = Guid.NewGuid();
            SetProjectId(projectId);
            SetUserId(userId);
            SetUserRole(userRole);
        }
        public void SetProjectId(Guid projectId)
        {
            if (projectId == Guid.Empty)
                throw new Exception("ProjectId_Cannot_Be_Empty");
            ProjectId = projectId;
        }
        public void SetUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new Exception("UserId_Cannot_Be_Empty");
            UserId = userId;
        }
        public void SetUserRole(ProjectRoleEnum userRole)
        {
            if (!Enum.IsDefined(typeof(ProjectRoleEnum), userRole))
                throw new Exception("Invalid_User_Role");
            UserRole = userRole;
        }
    }
}
