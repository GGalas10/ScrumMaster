using ScrumMaster.Tasks.Infrastructure.DTOs.Comments;

namespace ScrumMaster.Tasks.Infrastructure.Contracts
{
    public interface ICommentService
    {
        Task<List<CommentDTO>> GetTaskComments(Guid taskId);
    }
}
