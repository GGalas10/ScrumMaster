using ScrumMaster.Tasks.Core.Enums;
using ScrumMaster.Tasks.Core.Models;

namespace ScrumMaster.Tasks.Infrastructure.DTOs
{
    public class TaskDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public StatusEnum Status { get; set; }
        public Guid AssignedUserId { get; set; }
        public Guid SprintId { get; set; }
        public static TaskDTO GetFromModel(TaskModel model)
        {
            if (model == null)
                return null;
            return new()
            {
                Title = model.Title,
                Description = model.Description,
                Status = model.Status,
                AssignedUserId = model.AssignedUserId,
                SprintId = model.SprintId
            };
        }
    }
}
