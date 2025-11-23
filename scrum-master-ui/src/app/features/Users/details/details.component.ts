import { Component, OnInit, signal } from '@angular/core';
import { UserService } from '../../../Core/Services/user.service';
import { ActivatedRoute } from '@angular/router';
import { UserDetails } from '../../../Core/Models/UsersInterfaces';
import { CommonModule } from '@angular/common';
import { LeftMenuComponent } from '../../../shared/left-menu/left-menu.component';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-details',
  imports: [CommonModule, LeftMenuComponent, TranslatePipe],
  templateUrl: './details.component.html',
  styleUrl: './details.component.scss',
})
export class DetailsComponent implements OnInit {
  userId = signal('');
  user?: UserDetails;
  constructor(
    private userService: UserService,
    private activatedRoute: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      this.userId.set(params['id']);
    });
    this.userService.GetUserDetails(this.userId()).subscribe({
      next: (result) => (this.user = result),
      error: (err) => {
        alert(err.message);
      },
    });
  }
}
