namespace ScrumMaster.Project.Infrastructure.DTOs
{
    public class BoardInfoDTO
    {
        public string projectName { get; set; }
        public string projectDescription { get; set; }
        public List<MemberDTO> members { get; set; }
    }
}
