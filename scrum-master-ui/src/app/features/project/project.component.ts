import { Component, OnInit } from '@angular/core';
import {
  CreateProject,
  UserProject,
} from '../../Core/Models/ProjectInterfaces';
import { ProjectService } from '../../Core/Services/project.service';
import { TranslatePipe } from '@ngx-translate/core';

import { FormsModule } from '@angular/forms';
import { NewProjectModalComponent } from '../../shared/new-project-modal/new-project-modal.component';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { ErrorModel } from '../../shared/ErrorClass';

@Component({
  selector: 'app-project',
  imports: [
    TranslatePipe,
    FormsModule,
    NewProjectModalComponent,
    RouterLink
],
  templateUrl: './project.component.html',
  styleUrl: './project.component.scss',
})
export class ProjectComponent implements OnInit {
  open = false;
  userProjects: UserProject[] = [];
  isLoading = true;
  errors = new ErrorModel();
  constructor(private projectService: ProjectService, private router: Router) {}
  ngOnInit(): void {
    this.projectService
      .GetUsersProject()
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe({
        next: (result) => {
          this.userProjects = result;
        },
        error: (response) => console.log(response),
      });
  }
  onCreate(command: CreateProject): void {
    this.projectService.CreateProject(command).subscribe({
      next: (result) => {
        this.router.navigate(['/Board', result]);
      },
      error: (err) => {
        alert(err);
      },
    });
  }
}
