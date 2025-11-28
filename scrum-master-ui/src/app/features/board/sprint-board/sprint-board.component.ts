import { Component, OnInit } from '@angular/core';
import { SprintService } from '../../../Core/Services/sprint.service';
import { TaskService } from '../../../Core/Services/task.service';
import { QueryParameterService } from '../../../shared/query-parameter.service';

@Component({
  selector: 'app-sprint-board',
  imports: [],
  templateUrl: './sprint-board.component.html',
  styleUrl: './sprint-board.component.scss',
})
export class SprintBoardComponent implements OnInit {
  projectId = '';
  sprintId = '';
  constructor(
    private sprintService: SprintService,
    private taskService: TaskService,
    private queryParameter: QueryParameterService
  ) {}
  ngOnInit(): void {
    this.projectId = this.queryParameter.getQueryParam('id');
    this.sprintId = this.queryParameter.getQueryParam('sprintId');
    console.log(this.sprintId);
    this.taskService.GetTaskStatuses().subscribe({
      next: (result) => {
        console.log(result);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
}
