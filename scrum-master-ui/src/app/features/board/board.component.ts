import { Component, OnInit, signal } from '@angular/core';
import { BoardService } from '../../Core/Services/Board.service';
import { LeftMenuComponent } from '../../shared/left-menu/left-menu.component';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-board',
  imports: [LeftMenuComponent, CommonModule, TranslatePipe],
  templateUrl: './board.component.html',
  styleUrl: './board.component.scss',
})
export class BoardComponent implements OnInit {
  projectId = signal('');
  projectDescription = '';
  isLoading = true;
  projectError = '';
  constructor(
    private activatedRoute: ActivatedRoute,
    private boardService: BoardService
  ) {
    this.activatedRoute.params.subscribe((params) => {
      this.projectId.set(params['id']);
    });
  }
  ngOnInit(): void {
    this.boardService
      .GetBoardInfo(this.projectId())
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe({
        next: (result) => {
          this.projectDescription = result;
        },
        error: (err) => {
          this.projectError = err.error;
        },
      });
  }
}
