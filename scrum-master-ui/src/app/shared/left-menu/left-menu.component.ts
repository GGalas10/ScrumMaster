import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';

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
}
