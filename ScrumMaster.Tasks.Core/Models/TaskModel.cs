using ScrumMaster.Tasks.Core.Enums;

namespace ScrumMaster.Tasks.Core.Models
{
    class TaskModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public StatusEnum Status { get; set; }
        public Guid AssignedUserId { get; set; }
        public Guid SprintId { get; set; }
        private TaskModel() { }
        public TaskModel(string title, string description, Guid assignedUser, Guid sprintId)
        {

            SetTitle(title);
            SetDescription(description);
            ChangeAssignedUser(assignedUser);
            ChangeSprint(sprintId);
            Status = StatusEnum.New;
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
                throw new Exception("UserId_Cannot_Be_Empty");
            SprintId = sprintId;
        }
        public bool UpdateTask(string newTitle,string newDescription,StatusEnum newStatus,Guid sprintId,Guid assignedUser)
        {
            bool anyChanges = false;
            anyChanges = UpdateTitle(newTitle);
            anyChanges = UpdateDescription(newDescription);
            anyChanges = UpdateAssignedUser(assignedUser);
            anyChanges = UpdateSprint(sprintId);
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
