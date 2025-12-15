import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { TaskService } from '../../../../Core/Services/task.service';
import { TaskDTO } from '../../../../Core/Models/TaskInterfaces';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-task-details',
  imports: [CommonModule],
  templateUrl: './task-details.component.html',
  styleUrl: './task-details.component.scss',
})
export class TaskDetailsComponent implements OnChanges {
  @Input() taskId!: string;
  @Input() isOpen = false;
  @Output() closeEvent = new EventEmitter<void>();
  form: FormGroup;
  taskDTO!: TaskDTO | null;
  constructor(private taskService: TaskService, private fb: FormBuilder) {
    this.form = this.fb.group({
      title: [this.taskDTO?.title, Validators.required],
      description: [this.taskDTO?.description, Validators.required],
      assignedUser: [this.taskDTO?.assignedUserId, Validators.required],
      status: [this.taskDTO?.status, Validators.required],
    });
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
  }
  CloseModal() {
    this.taskDTO = null;
    this.closeEvent.emit();
  }
}
