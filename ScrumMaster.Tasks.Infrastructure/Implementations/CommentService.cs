using ScrumMaster.Tasks.Infrastructure.Contracts;
using ScrumMaster.Tasks.Infrastructure.DTOs.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumMaster.Tasks.Infrastructure.Implementations
{
    internal class CommentService : ICommentService
    {
        public Task<List<CommentDTO>> GetTaskComments(Guid taskId)
        {
            throw new NotImplementedException();
        }
    }
}
