export interface SprintList {
  id: string;
  name: string;
  isActual: boolean;
}
export interface SprintDTO {
  sprintId: string;
  sprintName: string;
  createBy: string;
  sprintStartAt: Date;
  sprintEndAt: Date;
}
export interface CreateSprintCommand {
  name: string;
  startDate: Date;
  endDate: Date;
  projectId: string;
}
