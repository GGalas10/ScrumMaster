export interface UserProject {
  projectId: string;
  projectName: string;
  userRole: number;
}
export interface CreateProject {
  projectName: string;
  projectDescription: string;
}
export interface ProjectMember {
  id: string;
  firstName: string;
  lastName: string;
}
