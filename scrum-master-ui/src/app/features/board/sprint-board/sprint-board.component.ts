import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../../Core/Services/task.service';
import { QueryParameterService } from '../../../shared/query-parameter.service';
import { LeftMenuComponent } from '../../../shared/left-menu/left-menu.component';
import {
  CreateTaskCommand,
  TaskListDTO,
  TaskStatuses,
} from '../../../Core/Models/TaskInterfaces';
import { CommonModule } from '../../../../../node_modules/@angular/common';
import { AddBtnComponent } from '../../../shared/add-btn/add-btn.component';
import { AddTaskComponent } from './add-task/add-task.component';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-sprint-board',
  imports: [
    LeftMenuComponent,
    CommonModule,
    AddBtnComponent,
    AddTaskComponent,
    TranslatePipe,
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
    this.taskService
      .GetAllSprintTasks(this.queryParameter.getQueryParam('sprintId'))
      .subscribe({
        next: (result) => (this.allTasks = result),
        error: (err) => console.log(err.error),
      });
  }
  AddTaskBtnClick(status: number) {
    this.addStatus = status;
    this.openModal = true;
  }
  getTasksByStatus(status: number): TaskListDTO[] {
    return (this.allTasks ?? []).filter((t) => t.status === status);
  }
  AddTask(command: CreateTaskCommand) {
    command.sprintId = this.sprintId;
    this.taskService.CreateTask(command).subscribe({
      next: (result) => console.log(result),
      error: (err) => console.log(err.error),
    });
  }
}
