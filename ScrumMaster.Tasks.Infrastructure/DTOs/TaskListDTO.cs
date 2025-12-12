using ScrumMaster.Tasks.Core.Enums;
using ScrumMaster.Tasks.Core.Models;

namespace ScrumMaster.Tasks.Infrastructure.DTOs
{
    public class TaskListDTO
    {
        public string Title { get; set; }
        public StatusEnum Status { get; set; }
        public string AssignedUserFullName { get; set; }
        public int TaskNumber { get; set; }
        public static TaskListDTO GetFromModel(TaskModel model)
        {
            if (model == null)
                return null;
            return new()
            {
                Title = model.Title,
                Status = model.Status,
                AssignedUserFullName = model.AssignedUser,
                TaskNumber = model.TaskNumber
            };
        }
    }
}
