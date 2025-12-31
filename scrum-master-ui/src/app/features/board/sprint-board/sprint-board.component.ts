import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../../Core/Services/task.service';
import { QueryParameterService } from '../../../shared/query-parameter.service';
import { LeftMenuComponent } from '../../../shared/left-menu/left-menu.component';
import {
  CreateTaskCommand,
  TaskListDTO,
  TaskStatuses,
} from '../../../Core/Models/TaskInterfaces';

import { AddBtnComponent } from '../../../shared/add-btn/add-btn.component';
import { AddTaskComponent } from './add-task/add-task.component';
import { TranslatePipe } from '@ngx-translate/core';
import { TaskDetailsComponent } from './task-details/task-details.component';
import {
  DragDropModule,
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem,
} from '@angular/cdk/drag-drop';
import { ErrorModel } from '../../../shared/ErrorClass';
import { CustomAlertComponent } from '../../../shared/components/custom-alert/custom-alert.component';

@Component({
  selector: 'app-sprint-board',
  imports: [
    LeftMenuComponent,
    AddBtnComponent,
    AddTaskComponent,
    TranslatePipe,
    TaskDetailsComponent,
    DragDropModule,
    CustomAlertComponent,
  ],
  templateUrl: './sprint-board.component.html',
  styleUrl: './sprint-board.component.scss',
})
export class SprintBoardComponent implements OnInit {
  projectId = '';
  sprintId = '';
  openModal = false;
  addStatus = 0;
  taskStatuses!: TaskStatuses[];
  allTasks!: TaskListDTO[];
  taskDetailsId = '';
  showDetailsModal = false;
  isLoading = true;
  errorModel: ErrorModel = new ErrorModel();
  constructor(
    private taskService: TaskService,
    private queryParameter: QueryParameterService
  ) {}
  ngOnInit(): void {
    this.projectId = this.queryParameter.getQueryParam('id');
    this.sprintId = this.queryParameter.getQueryParam('sprintId');
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
  AddTaskBtnClick(status: number) {
    this.addStatus = status;
    this.openModal = true;
  }
  getTasksByStatus(status: number): TaskListDTO[] {
    return (this.allTasks ?? []).filter((t) => t.status === status);
  }
  AddTask(command: CreateTaskCommand) {
    console.log(command);
    command.sprintId = this.sprintId;
    this.taskService.CreateTask(command).subscribe({
      next: () => {
        this.RefreshTasks();
      },
      error: (err) => console.log(err.error),
    });
  }
  ShowDetails(taskId: string) {
    this.showDetailsModal = true;
    this.taskDetailsId = taskId;
  }
  drop(event: CdkDragDrop<TaskListDTO[]>, targetStatus: number) {
    if (event.previousContainer === event.container) {
      const columnTasks = this.getTasksByStatus(targetStatus);
      moveItemInArray(columnTasks, event.previousIndex, event.currentIndex);

      this.rewriteColumnOrder(targetStatus, columnTasks);
      return;
    }

    const fromStatusId = this.parseStatusId(event.previousContainer.id);

    const fromTasks = this.getTasksByStatus(fromStatusId);
    const toTasks = this.getTasksByStatus(targetStatus);

    transferArrayItem(
      fromTasks,
      toTasks,
      event.previousIndex,
      event.currentIndex
    );

    const moved = toTasks[event.currentIndex];
    moved.status = targetStatus;
    this.UpdateTaskStatus(moved.id, targetStatus);

    this.rewriteColumnOrder(fromStatusId, fromTasks);
    this.rewriteColumnOrder(targetStatus, toTasks);
  }
  listId(statusId: number) {
    return `list-${statusId}`;
  }

  get listIds(): string[] {
    return this.taskStatuses.map((s) => this.listId(s.statusOrder));
  }
  connectedTo(statusId: number): string[] {
    const mine = this.listId(statusId);
    return this.listIds.filter((id) => id !== mine);
  }
  private rewriteColumnOrder(
    statusId: number,
    orderedColumnTasks: TaskListDTO[]
  ) {
    const others = this.allTasks.filter((t) => t.status !== statusId);
    this.allTasks = [...others, ...orderedColumnTasks];
  }
  private parseStatusId(dropListId: string): number {
    return Number(dropListId.replace('list-', ''));
  }
  UpdateTaskStatus(taskId: string, newStatus: number) {
    this.isLoading = true;
    this.taskService.UpdateTaskStatus(taskId, newStatus).subscribe({
      next: () => {
        const task = this.allTasks.find((t) => t.id === taskId);
        if (task) {
          task.status = newStatus;
        }
        this.isLoading = false;
      },
      error: () => {
        this.errorModel.showOneInternal();
        this.isLoading = false;
      },
    });
  }
  RefreshTasks() {
    this.taskService
      .GetAllSprintTasks(this.queryParameter.getQueryParam('sprintId'))
      .subscribe({
        next: (result) => {
          this.allTasks = result;
          this.isLoading = false;
        },
        error: (err) => {
          console.log(err.error);
          this.isLoading = false;
        },
      });
  }
  DeleteTask() {
    this.RefreshTasks();
    this.showDetailsModal = false;
  }
}
