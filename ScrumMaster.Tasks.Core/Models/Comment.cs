using ScrumMaster.Tasks.Core.Exceptions;

namespace ScrumMaster.Tasks.Core.Models
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public Guid SenderId { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Comment(Guid senderId, string content)
        {
            Id = Guid.NewGuid();
            SetSenderId(senderId);
            SetContent(content);
            CreatedAt = DateTime.UtcNow;
        }
        public void SetContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new BadRequestException("Content_Cannot_Be_Null_Or_Empty");
            Content = content;
        }
        public void SetSenderId(Guid senderId)
        {
            if(senderId == Guid.Empty)
                throw new BadRequestException("SenderId_Cannot_Be_Empty");
            SenderId = senderId;
        }
    }
}
