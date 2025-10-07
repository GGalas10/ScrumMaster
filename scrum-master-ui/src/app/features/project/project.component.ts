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

@Component({
  selector: 'app-project',
  imports: [TranslatePipe, CommonModule, FormsModule, NewProjectModalComponent],
  templateUrl: './project.component.html',
  styleUrl: './project.component.scss',
})
export class ProjectComponent implements OnInit {
  open = false;
  userProjects: UserProject[] = [];
  constructor(private projectService: ProjectService) {}
  ngOnInit(): void {
    this.projectService.GetUsersProject().subscribe({
      next: (result) => {
        this.userProjects = result;
      },
      error: (response) => console.log(response),
    });
  }
  onCreate(command: CreateProject): void {
    console.log(command);
  }
}
