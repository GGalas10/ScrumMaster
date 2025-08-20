import { Component, OnInit } from '@angular/core';
import { BoardService } from '../../Core/Services/Board.service';
import { LeftMenuComponent } from '../../shared/left-menu/left-menu.component';
import { TaskService } from '../../Core/Services/task.service';
import { TaskStatuses } from '../../Core/Models/TaskInterfaces';
import { CommonModule } from '@angular/common';
import { AddBtnComponent } from '../../shared/add-btn/add-btn.component';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-board',
  imports: [
    LeftMenuComponent,
    CommonModule,
    AddBtnComponent,
    AddBtnComponent,
    TranslatePipe,
  ],
  templateUrl: './board.component.html',
  styleUrl: './board.component.scss',
})
export class BoardComponent implements OnInit {
  taskStatuses: TaskStatuses[] = [];
  constructor(
    private boardService: BoardService,
    private taskService: TaskService
  ) {}
  ngOnInit(): void {
    this.boardService.GetBoardInfo().subscribe({
      error: (response) => console.log(response),
    });
    this.taskService.GetTaskStatuses().subscribe({
      next: (response) => {
        this.taskStatuses = response;
        console.log(this.taskStatuses);
      },
      error: (err) => console.log(err),
    });
  }
  AddNewTask(): void {
    console.log('TaskDodany');
  }
}
