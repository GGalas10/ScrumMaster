using Microsoft.EntityFrameworkCore;
using ScrumMaster.Tasks.Core.Extenstions;
using ScrumMaster.Tasks.Core.Models;
using ScrumMaster.Tasks.Infrastructure.Commands;
using ScrumMaster.Tasks.Infrastructure.Contracts;
using ScrumMaster.Tasks.Infrastructure.DataAccess;
using ScrumMaster.Tasks.Infrastructure.DTOs.Comments;
using ScrumMaster.Tasks.Infrastructure.Exceptions;
using System.Threading.Tasks;

namespace ScrumMaster.Tasks.Infrastructure.Implementations
{
    public class CommentService : ICommentService
    {
        private ITaskDbContext _taskDbContext;
        public CommentService(ITaskDbContext taskDbContext)
        {
            _taskDbContext = taskDbContext;
        }

        public async Task<List<CommentDTO>> GetTaskComments(Guid taskId,Guid senderId)
        {
            if (taskId == Guid.Empty)
                throw new BadRequestException("TaskId_Cannot_Be_Empty");
            var comments = await _taskDbContext.Comments.Where(x => x.taskId == taskId).ToListAsync();
            return comments.Select(x => new CommentDTO
            {
                id = x.Id,
                content = x.Content,
                senderId = x.SenderId,
                fromSender = x.SenderId == senderId
            }).ToList();
        }
        public async Task SendComment(CreateCommentCommand command)
        {
            if (command.taskId == Guid.Empty)
                throw new BadRequestException("TaskId_Cannot_Be_Empty");
            if (string.IsNullOrWhiteSpace(command.content))
                throw new BadRequestException("Content_Cannot_Be_Null");
            command.content = command.content.DeleteDangerousChars();
            _taskDbContext.Comments.Add(command.CreateComment());
            await _taskDbContext.SaveChangesAsync();
        }
    }
}
