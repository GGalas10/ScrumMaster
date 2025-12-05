import { Component, OnInit } from '@angular/core';
import { SprintService } from '../../../Core/Services/sprint.service';
import { TaskService } from '../../../Core/Services/task.service';
import { QueryParameterService } from '../../../shared/query-parameter.service';
import { LeftMenuComponent } from '../../../shared/left-menu/left-menu.component';
import {
  CreateTaskCommand,
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
  constructor(
    private sprintService: SprintService,
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
  }
  AddTaskBtnClick(status: number) {
    this.addStatus = status;
    this.openModal = true;
  }
  AddTask(command: CreateTaskCommand) {
    command.sprintId = this.sprintId;
    console.log(command);
  }
}
