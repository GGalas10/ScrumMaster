import { Component, OnInit } from '@angular/core';
import {
  CreateProject,
  UserProject,
} from '../../Core/Models/ProjectInterfaces';
import { ProjectService } from '../../Core/Services/project.service';
import { TranslatePipe } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NewProjectModalComponent } from '../../shared/new-project-modal/new-project-modal.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-project',
  imports: [TranslatePipe, CommonModule, FormsModule, NewProjectModalComponent],
  templateUrl: './project.component.html',
  styleUrl: './project.component.scss',
})
export class ProjectComponent implements OnInit {
  open = false;
  userProjects: UserProject[] = [];
  constructor(private projectService: ProjectService, private router: Router) {}
  ngOnInit(): void {
    this.projectService.GetUsersProject().subscribe({
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
