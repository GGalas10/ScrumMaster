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
export interface AddMemeberCommand {
  projectId: string;
  userId: string;
  roleEnum: number;
}
export enum ProjectRoleEnum {
  Owner = 1,
  Admin = 2,
  Member = 3,
  Guest = 4,
  Observer = 5,
  Custom = 6,
}
