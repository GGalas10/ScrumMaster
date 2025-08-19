namespace ScrumMaster.Project.Core.Models
{
    public class ProjectSettings
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public ProjectModel Project { get; private set; }
        private ProjectSettings() { }
        public ProjectSettings(Guid projectId, ProjectModel project)
        {
            Id = Guid.NewGuid();
            SetProjectId(projectId);
            SetProject(project);
        }
        public void SetProjectId(Guid projectId)
        {
            if (projectId == Guid.Empty)
                throw new Exception("ProjectId_Cannot_Be_Empty");
            ProjectId = projectId;
        }
        public void SetProject(ProjectModel project)
        {
            if (project == null)
                throw new Exception("Project_Cannot_Be_Null");
            if (project.Id == Guid.Empty)
                throw new Exception("ProjectId_Cannot_Be_Empty");
            Project = project;

        }
    }
}
