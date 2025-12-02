import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SprintService } from '../../Core/Services/sprint.service';

@Component({
  selector: 'app-left-menu',
  imports: [CommonModule, TranslatePipe],
  templateUrl: './left-menu.component.html',
  styleUrl: './left-menu.component.scss',
})
export class LeftMenuComponent {
  sprintMenu = false;
  boardMenu = false;
  AIAssistant = false;
  projectId = signal('');
  actualSprintId = signal('');
  routerLinkValue = signal('');
  constructor(
    private activatedRoute: ActivatedRoute,
    private sprintService: SprintService,
    private router: Router
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
  setSuccessLink() {
    this.routerLinkValue.set(
      `/Sprint/${this.projectId()}/Board/${this.actualSprintId()}`
    );
  }
  setErrorLink() {
    this.routerLinkValue.set(`/Sprint/${this.projectId()}`);
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
  SprintTaskLink() {
    if (this.actualSprintId() == null) {
      this.setErrorLink();
    }
    this.RouterLink(this.routerLinkValue());
  }
  SprintLink() {
    this.RouterLink(`/Sprint/${this.projectId()}`);
  }
  HomeLink() {
    this.RouterLink(`/Board/${this.projectId()}`);
  }
  private RouterLink(link: string) {
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigateByUrl(link);
    });
  }
}
