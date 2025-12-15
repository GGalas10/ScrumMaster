export interface TaskStatuses {
  statusOrder: number;
  statusName: string;
}
export interface CreateTaskCommand {
  title: string;
  description: string;
  sprintId: string;
  status: number;
}
export interface TaskDTO {
  title: string;
  description: string;
  status: number;
  assignedUserId: string;
  createById: string;
  sprintId: string;
  createBy: string;
  assignedUserFullName: string;
  taskNumber: number;
  createdAt: Date;
  updatedAt: Date;
}
export interface TaskListDTO {
  id: string;
  title: string;
  status: number;
  assignedUserFullName: string;
  taskNumber: number;
}
