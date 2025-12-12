import { Component, OnInit, signal } from '@angular/core';
import { BoardService } from '../../Core/Services/Board.service';
import { LeftMenuComponent } from '../../shared/left-menu/left-menu.component';

import { TranslatePipe } from '@ngx-translate/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { finalize } from 'rxjs';
import { BoardDto } from '../../Core/Models/BoardInterfaces';
import { QueryParameterService } from '../../shared/query-parameter.service';

@Component({
  selector: 'app-board',
  imports: [LeftMenuComponent, TranslatePipe, RouterModule],
  templateUrl: './board.component.html',
  styleUrl: './board.component.scss',
})
export class BoardComponent implements OnInit {
  boardDTO!: BoardDto;
  isLoading = true;
  projectError = '';
  constructor(
    private activatedRoute: ActivatedRoute,
    private boardService: BoardService,
    private queryParameter: QueryParameterService
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
  }
}
