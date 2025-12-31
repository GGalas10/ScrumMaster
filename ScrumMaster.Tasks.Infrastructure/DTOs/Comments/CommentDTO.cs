namespace ScrumMaster.Tasks.Infrastructure.DTOs.Comments
{
    public class CommentDTO
    {
        public Guid id { get; set; }
        public string content { get; set; }
        public Guid senderId { get; set; }
        public bool fromSender { get; set; }
    }
}
