import { Component, OnInit, signal } from '@angular/core';
import { LeftMenuComponent } from '../../../shared/left-menu/left-menu.component';
import {
  CreateSprintCommand,
  SprintDTO,
} from '../../../Core/Models/SprintInterfaces';
import { SprintService } from '../../../Core/Services/sprint.service';
import { QueryParameterService } from '../../../shared/query-parameter.service';
import { CommonModule } from '@angular/common';
import { AddBtnComponent } from '../../../shared/add-btn/add-btn.component';
import { AddSprintComponent } from './add-sprint/add-sprint.component';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-sprint',
  imports: [
    LeftMenuComponent,
    CommonModule,
    AddBtnComponent,
    AddSprintComponent,
    TranslatePipe,
  ],
  templateUrl: './sprint.component.html',
  styleUrl: './sprint.component.scss',
})
export class SprintComponent implements OnInit {
  spintsDTO!: SprintDTO[];
  isModalOpen = false;
  ngOnInit(): void {
    this.sprintService
      .GetProjectSprints(this.queryParameter.getQueryParam('id'))
      .subscribe({
        next: (result) => {
          this.spintsDTO = result;
        },
        error: (err) => console.log(err),
      });
  }
  constructor(
    private sprintService: SprintService,
    private queryParameter: QueryParameterService
  ) {}
  CreateSprint(command: CreateSprintCommand) {
    this.sprintService.CreateSprint(command);
    this.sprintService
      .GetProjectSprints(this.queryParameter.getQueryParam('id'))
      .subscribe({
        next: (result) => {
          this.spintsDTO = result;
        },
        error: (err) => console.log(err),
      });
  }
}
