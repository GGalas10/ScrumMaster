import { Component, EventEmitter, Output } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { AuthService } from '../../Core/Services/Auth.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-dropdown',
  imports: [TranslatePipe, RouterModule],
  templateUrl: './dropdown.component.html',
  styleUrl: './dropdown.component.scss',
})
export class DropdownComponent {
  @Output() closeEvent = new EventEmitter<void>();
  constructor(private authService: AuthService, private router: Router) {}
  logout(): void {
    this.closeEvent.emit();
    this.authService.Logout().subscribe({
      next: () => {
        this.router.navigateByUrl('/');
      },
      error: () => {
        this.router.navigateByUrl('/');
      },
    });
  }
}
