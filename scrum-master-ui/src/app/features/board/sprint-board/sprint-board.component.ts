import { Component, OnInit } from '@angular/core';
import { LeftMenuComponent } from '../../../shared/left-menu/left-menu.component';
import { TaskListDTO } from '../../../Core/Models/TaskInterfaces';
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
import { CustomAlertComponent } from '../../../shared/components/custom-alert/custom-alert.component';
import { BoardState } from './States/board.state';

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
  boardState = new BoardState();
  constructor() {}
  ngOnInit(): void {}
  drop(event: CdkDragDrop<TaskListDTO[]>, targetStatus: number) {
    if (event.previousContainer === event.container) {
      const columnTasks = this.boardState.getTasksByStatus(targetStatus);
      moveItemInArray(columnTasks, event.previousIndex, event.currentIndex);

      this.rewriteColumnOrder(targetStatus, columnTasks);
      return;
    }

    const fromStatusId = this.parseStatusId(event.previousContainer.id);

    const fromTasks = this.boardState.getTasksByStatus(fromStatusId);
    const toTasks = this.boardState.getTasksByStatus(targetStatus);

    transferArrayItem(
      fromTasks,
      toTasks,
      event.previousIndex,
      event.currentIndex
    );

    const moved = toTasks[event.currentIndex];
    moved.status = targetStatus;
    this.boardState.UpdateTaskStatus(moved.id, targetStatus);

    this.rewriteColumnOrder(fromStatusId, fromTasks);
    this.rewriteColumnOrder(targetStatus, toTasks);
  }
  private rewriteColumnOrder(
    statusId: number,
    orderedColumnTasks: TaskListDTO[]
  ) {
    const others = this.boardState.allTasks.filter(
      (t) => t.status !== statusId
    );
    this.boardState.allTasks = [...others, ...orderedColumnTasks];
  }
  private parseStatusId(dropListId: string): number {
    return Number(dropListId.replace('list-', ''));
  }
}
