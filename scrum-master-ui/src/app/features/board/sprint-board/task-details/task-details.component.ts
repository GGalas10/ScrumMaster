import {
  Component,
  EventEmitter,
  HostListener,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { TaskService } from '../../../../Core/Services/task.service';
import { TaskDTO } from '../../../../Core/Models/TaskInterfaces';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  Validators,
  ɵInternalFormsSharedModule,
} from '@angular/forms';
import { ProjectService } from '../../../../Core/Services/project.service';
import { QueryParameterService } from '../../../../shared/query-parameter.service';
import { ProjectMember } from '../../../../Core/Models/ProjectInterfaces';
import { CommentService } from '../../../../Core/Services/comment.service';
import {
  Comment,
  CreateCommentCommand,
} from '../../../../Core/Models/TaskCommentInterfaces';
import { AcceptAlertComponent } from '../../../../shared/components/accept-alert/accept-alert.component';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-task-details',
  imports: [
    CommonModule,
    ɵInternalFormsSharedModule,
    FormsModule,
    AcceptAlertComponent,
    TranslatePipe,
  ],
  templateUrl: './task-details.component.html',
  styleUrl: './task-details.component.scss',
})
export class TaskDetailsComponent implements OnChanges {
  @Input() taskId!: string;
  @Input() isOpen = false;
  @Output() closeEvent = new EventEmitter<void>();
  @Output() deleteEvent = new EventEmitter<void>();
  form: FormGroup;
  projectMembers!: ProjectMember[];
  taskDTO!: TaskDTO | null;
  taskComments!: Comment[] | null;
  taskContent = '';
  openModal = false;
  constructor(
    private taskService: TaskService,
    private fb: FormBuilder,
    private projectService: ProjectService,
    private queryParameter: QueryParameterService,
    private commentService: CommentService
  ) {
    this.form = this.fb.group({
      title: [this.taskDTO?.title, Validators.required],
      description: [this.taskDTO?.description, Validators.required],
      assignedUser: [this.taskDTO?.assignedUserId, Validators.required],
      status: [this.taskDTO?.status, Validators.required],
    });
  }
  @HostListener('document:keydown.escape')
  onEsc() {
    this.CloseModal();
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen'] && this.isOpen == true) {
      this.Refresh();
    }
  }
  Refresh() {
    this.taskService.GetTaskDetails(this.taskId).subscribe({
      next: (result) => {
        this.taskDTO = result;
        this.form = this.fb.group({
          title: [this.taskDTO?.title, Validators.required],
          description: [this.taskDTO?.description, Validators.required],
          assignedUser: [this.taskDTO?.assignedUserId, Validators.required],
          status: [this.taskDTO?.status, Validators.required],
        });
      },
      error: (err) => console.log(err),
    });
    this.projectService
      .GetProjectMembers(this.queryParameter.getQueryParam('id'))
      .subscribe({
        next: (result) => {
          this.projectMembers = result;
        },
      });
    this.RefreshComments();
  }
  CloseModal() {
    this.taskDTO = null;
    this.closeEvent.emit();
  }
  RefreshComments() {
    this.commentService.GetCommentByTaskId(this.taskId).subscribe({
      next: (result) => (this.taskComments = result),
      error: (err) => (this.taskComments = null),
    });
  }
  SendComment() {
    var command: CreateCommentCommand = {
      taskId: this.taskId,
      content: this.taskContent,
    };
    this.commentService.SendComment(command).subscribe({
      next: () => {
        this.RefreshComments();
        this.taskContent = '';
      },
      error: (err) => {
        console.log(err.error);
      },
    });
  }
  GetCommentSender(senderId: string) {
    var sender = this.projectMembers.find((x) => x.id == senderId);
    if (sender) {
      return `${sender?.firstName} ${sender?.lastName}`;
    }
    return 'unknown';
  }
  AskModal() {
    this.openModal = true;
  }
  DeleteTask(result: boolean) {
    if (!result) {
      this.openModal = false;
      return;
    }
    this.taskService.DeleteTask(this.taskId).subscribe({
      next: () => {
        this.taskDTO = null;
        this.deleteEvent.emit();
      },
    });
  }
}
