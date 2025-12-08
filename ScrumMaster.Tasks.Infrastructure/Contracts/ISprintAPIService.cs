namespace ScrumMaster.Tasks.Infrastructure.Contracts
{
    public interface ISprintAPIService
    {
        Task CheckSprintExist(Guid sprintId);
        Task<Guid> GetProjectIdBySprintId(Guid sprintId);
    }
}
