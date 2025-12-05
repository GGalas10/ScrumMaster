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
