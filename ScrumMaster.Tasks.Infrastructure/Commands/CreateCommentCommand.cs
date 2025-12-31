using ScrumMaster.Tasks.Core.Models;

namespace ScrumMaster.Tasks.Infrastructure.Commands
{
    public class CreateCommentCommand
    {
        public Guid senderId { get; set; }
        public string content { get; set; }
        public Guid taskId { get; set; }
        public Comment CreateComment()
        {
            return new Comment(senderId, content, taskId);
        }
    }
}
