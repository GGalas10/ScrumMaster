using ScrumMaster.Tasks.Core.Enums;
using ScrumMaster.Tasks.Core.Exceptions;

namespace ScrumMaster.Tasks.Core.Models
{
    public class TaskModel
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public StatusEnum Status { get; private set; }
        public Guid AssignedUserId { get; private set; }
        public Guid SprintId { get; private set; }
        public Guid CreateById { get; private set; }
        public string CreatedBy { get; private set; }
        public string AssignedUser { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        private TaskModel() { }
        public TaskModel(string title, string description, Guid sprintId, Guid createdById,string createdBy, StatusEnum status = StatusEnum.New)
        {

            SetTitle(title);
            SetDescription(description);
            ChangeSprint(sprintId);
            SetCreatedBy(createdById,createdBy);
            Status = status;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            AssignedUser = "";
            AssignedUserId = Guid.Empty;
            Id = Guid.NewGuid();
        }
        public void SetTitle(string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle))
                throw new BadRequestException("Title_Cannot_Be_Null_Or_Empty");
            Title = newTitle;
        }
        public void SetDescription(string newDescription)
        {
            if (string.IsNullOrEmpty(newDescription))
                throw new BadRequestException("Description_Cannot_Be_Null_Or_Empty");
            Description = newDescription;
        }
        public void ChangeAssignedUser(Guid newUserId)
        {
            if (newUserId == Guid.Empty)
                throw new BadRequestException("UserId_Cannot_Be_Empty");
            AssignedUserId = newUserId;
        }
        public void ChangeSprint(Guid sprintId)
        {
            if (sprintId == Guid.Empty)
                throw new BadRequestException("SprintId_Cannot_Be_Empty");
            SprintId = sprintId;
        }
        public void SetCreatedBy(Guid userId,string createdBy)
        {
            if (userId == Guid.Empty)
                throw new BadRequestException("CreatedById_Cannot_Be_Empty");
            if(string.IsNullOrWhiteSpace(createdBy))
                throw new BadRequestException("CreatedBy_Cannot_Be_Null_Or_Empty");
            CreateById = userId;
            CreatedBy = createdBy;
        }
        public bool UpdateTask(string newTitle,string newDescription,StatusEnum newStatus,Guid sprintId,Guid assignedUserId, string assignedUser)
        {
            bool anyChanges = false;
            anyChanges = UpdateTitle(newTitle) ? true : anyChanges;
            anyChanges = UpdateDescription(newDescription) ? true : anyChanges;
            anyChanges = UpdateAssignedUser(assignedUserId,assignedUser) ? true : anyChanges;
            anyChanges = UpdateSprint(sprintId) ? true : anyChanges;
            if(Status != newStatus)
            {
                anyChanges = true;
                Status = newStatus;
                UpdatedAt = DateTime.UtcNow;
            }
            return anyChanges;
        }
        private bool UpdateTitle(string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle) || Title == newTitle)
                return false;
            Title = newTitle;
            return true;
        }
        private bool UpdateDescription(string newDescription)
        {
            if (string.IsNullOrEmpty(newDescription) || Description == newDescription)
                return false;
            Description = newDescription;
            return true;
        }
        private bool UpdateAssignedUser(Guid newAssignedUser, string assignedUser)
        {
            if (newAssignedUser == Guid.Empty || AssignedUserId == newAssignedUser)
                return false;
            if(string.IsNullOrWhiteSpace(assignedUser))
                throw new BadRequestException("AssignedUser_Cannot_Be_Null_Or_Empty");
            AssignedUserId = newAssignedUser;
            return true;
        }
        private bool UpdateSprint(Guid sprintId)
        {
            if (sprintId == Guid.Empty || SprintId == sprintId)
                return false;
            SprintId = sprintId;
            return true;
        }
    }
}
