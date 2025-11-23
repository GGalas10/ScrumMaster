namespace ScrumMaster.Project.Infrastructure.DTOs
{
    public class MemberDTO
    {
        public Guid id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string shortcut => GetShortcut();
        private string GetShortcut()
        {
            var first = string.IsNullOrWhiteSpace(firstName) ? "" : $"{firstName[0]}";
            var last = string.IsNullOrWhiteSpace(firstName) ? "" : $"{lastName[0]}";
            return string.IsNullOrWhiteSpace($"{first}{last}") ? "Un" : $"{first}{last}";
        }
    }
}
