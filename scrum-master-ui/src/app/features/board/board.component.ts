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
  imports: [LeftMenuComponent, CommonModule, TranslatePipe],
  templateUrl: './board.component.html',
  styleUrl: './board.component.scss',
})
export class BoardComponent implements OnInit {
  constructor() {}
  ngOnInit(): void {}
}
