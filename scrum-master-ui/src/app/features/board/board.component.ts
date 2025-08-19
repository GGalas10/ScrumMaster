import { Component, OnInit } from '@angular/core';
import { BoardService } from '../../Core/Services/Board.service';
import { LeftMenuComponent } from '../../shared/left-menu/left-menu.component';
import { TaskService } from '../../Core/Services/task.service';
import { TaskStatuses } from '../../Core/Models/TaskInterfaces';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-board',
  imports: [LeftMenuComponent, CommonModule],
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
}
