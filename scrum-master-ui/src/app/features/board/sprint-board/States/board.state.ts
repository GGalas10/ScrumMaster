import { inject, signal } from '@angular/core';
import {
  CreateTaskCommand,
  TaskListDTO,
  TaskStatuses,
} from '../../../../Core/Models/TaskInterfaces';
import { ErrorModel } from '../../../../shared/ErrorClass';
import { QueryParameterService } from '../../../../shared/query-parameter.service';
import { TaskService } from '../../../../Core/Services/task.service';

export class BoardState {
  private readonly queryParams = inject(QueryParameterService);
  private readonly taskService = inject(TaskService);
  projectId = '';
  sprintId = '';
  openModal = signal(false);
  addStatus = 0;
  taskStatuses!: TaskStatuses[];
  allTasks!: TaskListDTO[];
  taskDetailsId = '';
  showDetailsModal = signal(false);
  isLoading = signal(true);
  errorModel: ErrorModel = new ErrorModel();
  constructor() {
    this.projectId = this.queryParams.getQueryParam('id');
    this.sprintId = this.queryParams.getQueryParam('sprintId');
    this.taskService.GetTaskStatuses().subscribe({
      next: (result) => {
        this.taskStatuses = result;
      },
      error: (err) => {
        console.log(err);
      },
    });
    this.RefreshTasks();
  }
  RefreshTasks() {
    this.taskService
      .GetAllSprintTasks(this.queryParams.getQueryParam('sprintId'))
      .subscribe({
        next: (result) => {
          this.allTasks = result;
          this.isLoading.set(false);
        },
        error: (err) => {
          console.log(err.error);
          this.isLoading.set(false);
        },
      });
  }
  DeleteTask() {
    this.RefreshTasks();
    this.showDetailsModal.set(false);
  }
  UpdateTaskStatus(taskId: string, newStatus: number) {
    this.isLoading.set(true);
    this.taskService.UpdateTaskStatus(taskId, newStatus).subscribe({
      next: () => {
        const task = this.allTasks.find((t) => t.id === taskId);
        if (task) {
          task.status = newStatus;
        }
        this.isLoading.set(false);
      },
      error: () => {
        this.errorModel.showOneInternal();
        this.isLoading.set(false);
      },
    });
  }
  getTasksByStatus(status: number): TaskListDTO[] {
    return (this.allTasks ?? []).filter((t) => t.status === status);
  }
  AddTask(command: CreateTaskCommand) {
    command.sprintId = this.sprintId;
    this.taskService.CreateTask(command).subscribe({
      next: () => {
        this.RefreshTasks();
      },
      error: (err) => console.log(err.error),
    });
  }
  AddTaskBtnClick(status: number) {
    this.addStatus = status;
    this.openModal.set(true);
  }
  ShowDetails(taskId: string) {
    this.showDetailsModal.set(true);
    this.taskDetailsId = taskId;
  }
  get listIds(): string[] {
    return this.taskStatuses.map((s) => this.listId(s.statusOrder));
  }
  listId(statusId: number) {
    return `list-${statusId}`;
  }

  connectedTo(statusId: number): string[] {
    const mine = this.listId(statusId);
    return this.listIds.filter((id) => id !== mine);
  }
}
