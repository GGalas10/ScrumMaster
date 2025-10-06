import { Component, OnInit } from '@angular/core';
import { BoardService } from '../../Core/Services/Board.service';
import { LeftMenuComponent } from '../../shared/left-menu/left-menu.component';
import { TaskService } from '../../Core/Services/task.service';
import { TaskStatuses } from '../../Core/Models/TaskInterfaces';
import { CommonModule } from '@angular/common';
import { AddBtnComponent } from '../../shared/add-btn/add-btn.component';
import { TranslatePipe } from '@ngx-translate/core';
import { SprintList } from '../../Core/Models/SprintInterfaces';
import { UserProject } from '../../Core/Models/ProjectInterfaces';

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
  userProjects: UserProject[] = [];
  constructor(private boardService: BoardService) {}
  ngOnInit(): void {
    this.boardService.GetUsersProject().subscribe({
      next: (result) => {
        this.userProjects = result;
      },
      error: (response) => console.log(response),
    });
  }
}
