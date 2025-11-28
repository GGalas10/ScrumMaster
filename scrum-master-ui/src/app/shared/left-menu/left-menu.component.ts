import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { SprintService } from '../../Core/Services/sprint.service';

@Component({
  selector: 'app-left-menu',
  imports: [CommonModule, TranslatePipe, RouterLink],
  templateUrl: './left-menu.component.html',
  styleUrl: './left-menu.component.scss',
})
export class LeftMenuComponent {
  sprintMenu = false;
  boardMenu = false;
  AIAssistant = false;
  projectId = signal('');
  actualSprintId = signal('');
  routerLinkValue = signal<any[]>([]);
  constructor(
    private activatedRoute: ActivatedRoute,
    private sprintService: SprintService
  ) {
    this.activatedRoute.params.subscribe((params) => {
      this.projectId.set(params['id']);
      this.actualSprintId.set(params['sprintId']);
    });
    if (this.actualSprintId() == undefined) {
      sprintService.GetActualSprint(this.projectId()).subscribe({
        next: (result) => {
          this.actualSprintId.set(result);
          this.setSuccessLink();
        },
        error: (err) => {
          this.setErrorLink();
        },
      });
    }
  }
  ShowSubMenu(name: string): void {
    switch (name) {
      case 'sprint':
        this.sprintMenu = !this.sprintMenu;
        this.boardMenu = false;
        this.AIAssistant = false;
        break;
      case 'board':
        this.sprintMenu = false;
        this.boardMenu = !this.boardMenu;
        this.AIAssistant = false;
        break;
      case 'ai':
        this.sprintMenu = false;
        this.boardMenu = false;
        this.AIAssistant = !this.AIAssistant;
        break;
    }
  }
  setSuccessLink() {
    this.routerLinkValue.set([
      '/Sprint',
      this.projectId,
      'SprintBoard',
      this.actualSprintId,
    ]);
  }

  setErrorLink() {
    this.routerLinkValue.set(['/Sprint', this.projectId()]);
  }
}
