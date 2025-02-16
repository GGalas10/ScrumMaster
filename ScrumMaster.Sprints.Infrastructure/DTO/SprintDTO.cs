using ScrumMaster.Sprints.Core.Models;

namespace ScrumMaster.Sprints.Infrastructure.DTO
{
    public class SprintDTO
    {
        public Guid sprintId { get; set; }
        public string sprintName { get; set; }
        public string createBy { get; set; }
        public DateTime sprintStartAt { get; set; }
        public DateTime sprintEndAt { get;set; }
        public static SprintDTO GetFromModel(Sprint model)
        {
            return new SprintDTO()
            {
                sprintId = model.Id,
                sprintName = model.Name,
                createBy = model.CreatedBy,
                sprintStartAt = model.StartDate,
                sprintEndAt = model.EndDate,
            };
        }
    }
}
