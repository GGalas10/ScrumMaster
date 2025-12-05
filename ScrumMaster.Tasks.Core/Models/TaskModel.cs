using ScrumMaster.Tasks.Core.Enums;

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
        private TaskModel() { }
        public TaskModel(string title, string description, Guid sprintId, StatusEnum status = StatusEnum.New)
        {

            SetTitle(title);
            SetDescription(description);
            ChangeSprint(sprintId);
            Status = status;
            Id = Guid.NewGuid();
        }
        public void SetTitle(string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle))
                throw new Exception("Title_Cannot_Be_Null_Or_Empty");
            Title = newTitle;
        }
        public void SetDescription(string newDescription)
        {
            if (string.IsNullOrEmpty(newDescription))
                throw new Exception("Description_Cannot_Be_Null_Or_Empty");
            Description = newDescription;
        }
        public void ChangeAssignedUser(Guid newUserId)
        {
            if (newUserId == Guid.Empty)
                throw new Exception("UserId_Cannot_Be_Empty");
            AssignedUserId = newUserId;
        }
        public void ChangeSprint(Guid sprintId)
        {
            if (sprintId == Guid.Empty)
                throw new Exception("SprintId_Cannot_Be_Empty");
            SprintId = sprintId;
        }
        public bool UpdateTask(string newTitle,string newDescription,StatusEnum newStatus,Guid sprintId,Guid assignedUser)
        {
            bool anyChanges = false;
            anyChanges = UpdateTitle(newTitle) ? true : anyChanges;
            anyChanges = UpdateDescription(newDescription) ? true : anyChanges;
            anyChanges = UpdateAssignedUser(assignedUser) ? true : anyChanges;
            anyChanges = UpdateSprint(sprintId) ? true : anyChanges;
            if(Status != newStatus)
            {
                anyChanges = true;
                Status = newStatus;
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
        private bool UpdateAssignedUser(Guid newAssignedUser)
        {
            if (newAssignedUser == Guid.Empty || AssignedUserId == newAssignedUser)
                return false;
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
