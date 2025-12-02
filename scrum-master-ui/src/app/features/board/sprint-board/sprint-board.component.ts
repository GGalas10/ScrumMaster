import { Component, OnInit } from '@angular/core';
import { SprintService } from '../../../Core/Services/sprint.service';
import { TaskService } from '../../../Core/Services/task.service';
import { QueryParameterService } from '../../../shared/query-parameter.service';
import { LeftMenuComponent } from '../../../shared/left-menu/left-menu.component';
import { TaskStatuses } from '../../../Core/Models/TaskInterfaces';
import { CommonModule } from '../../../../../node_modules/@angular/common';

@Component({
  selector: 'app-sprint-board',
  imports: [LeftMenuComponent, CommonModule],
  templateUrl: './sprint-board.component.html',
  styleUrl: './sprint-board.component.scss',
})
export class SprintBoardComponent implements OnInit {
  projectId = '';
  sprintId = '';
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
}
