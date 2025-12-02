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
import { Router } from '@angular/router';
import { CustomAlertComponent } from '../../../shared/components/custom-alert/custom-alert.component';
import { ErrorModel } from '../../../shared/ErrorClass';

@Component({
  selector: 'app-sprint',
  imports: [
    LeftMenuComponent,
    CommonModule,
    AddBtnComponent,
    AddSprintComponent,
    TranslatePipe,
    CustomAlertComponent,
  ],
  templateUrl: './sprint.component.html',
  styleUrl: './sprint.component.scss',
})
export class SprintComponent implements OnInit {
  spintsDTO!: SprintDTO[];
  isModalOpen = false;
  errorModel = new ErrorModel();
  ngOnInit(): void {
    this.Refresh();
  }
  constructor(
    private sprintService: SprintService,
    private queryParameter: QueryParameterService,
    private router: Router
  ) {}
  CreateSprint(command: CreateSprintCommand) {
    command.projectId = this.queryParameter.getQueryParam('id');
    this.sprintService.CreateSprint(command).subscribe({
      next: (result) => {
        this.router.navigateByUrl(
          `/Sprint/${this.queryParameter.getQueryParam('id')}/Board/${result}`
        );
      },
      error: (err) => {
        this.errorModel.showOneBadRequest(err.error, 'Errors.GeneralTitle');
      },
    });
    this.Refresh();
  }
  Refresh() {
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
