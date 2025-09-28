namespace ScrumMaster.Project.Core.Models
{
    public class ProjectModel
    {
        public Guid Id { get; private set; }
        public string ProjectName { get; private set; }
        public string ProjectDescription { get; private set; }
        private ProjectModel() { }
        public ProjectModel(string projectName, string projectDescription)
        {
            Id = Guid.NewGuid();
            SetProjectName(projectName);
            SetProjectDescription(projectDescription);
        }
        private void SetProjectName(string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
                throw new Exception("ProjectName_Cannot_Be_Empty_Or_Null");
            ProjectName = projectName;
        }
        private void SetProjectDescription(string projectDescription)
        {
            if (string.IsNullOrEmpty(projectDescription))
                throw new Exception("ProjectDescription_Cannot_Be_Empty_Or_Null");
            ProjectDescription = projectDescription;
        }
        public void UpdateProject(string projectName, string projectDescription)
        {
            SetProjectName(projectName);
            SetProjectDescription(projectDescription);
        }
    }
}
