import { Component, OnInit, signal } from '@angular/core';
import { BoardService } from '../../Core/Services/Board.service';
import { LeftMenuComponent } from '../../shared/left-menu/left-menu.component';

import { TranslatePipe } from '@ngx-translate/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { finalize } from 'rxjs';
import { BoardDto } from '../../Core/Models/BoardInterfaces';
import { QueryParameterService } from '../../shared/query-parameter.service';
import { AddBtnComponent } from '../../shared/add-btn/add-btn.component';
import { MangeMembersComponent } from './mange-members/mange-members.component';
import { ProjectService } from '../../Core/Services/project.service';

@Component({
  selector: 'app-board',
  imports: [
    LeftMenuComponent,
    TranslatePipe,
    RouterModule,
    AddBtnComponent,
    MangeMembersComponent,
  ],
  templateUrl: './board.component.html',
  styleUrl: './board.component.scss',
})
export class BoardComponent implements OnInit {
  boardDTO!: BoardDto;
  isLoading = true;
  projectError = '';
  openNewMember = false;
  CanManageMembers = signal(false);
  constructor(
    private boardService: BoardService,
    private queryParameter: QueryParameterService,
    private projectService: ProjectService
  ) {}
  ngOnInit(): void {
    this.boardService
      .GetBoardInfo(this.queryParameter.getQueryParam('id'))
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe({
        next: (result) => {
          this.boardDTO = result;
        },
        error: (err) => {
          this.projectError = err.error;
        },
      });
    this.projectService
      .CanManageMembers(this.queryParameter.getQueryParam('id'))
      .subscribe({
        next: (result) => {
          this.CanManageMembers.set(result);
        },
        error: (err) => {
          console.error('Error checking manage members permission', err);
        },
      });
  }
}
