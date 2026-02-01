import {
  Component,
  EventEmitter,
  HostListener,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { CreateTaskCommand } from '../../../../Core/Models/TaskInterfaces';

@Component({
  selector: 'app-add-task',
  imports: [ReactiveFormsModule, TranslatePipe],
  templateUrl: './add-task.component.html',
  styleUrl: './add-task.component.scss',
})
export class AddTaskComponent implements OnChanges {
  @Input() isOpen = false;
  @Input() status!: number;
  @Input() data: CreateTaskCommand | null = null;
  @Output() submitted = new EventEmitter<CreateTaskCommand>();
  @Output() closed = new EventEmitter<void>();
  form: FormGroup;
  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      status: [0, Validators.required],
    });
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen']) {
      this.form.get('status')?.setValue(this.status);
    }
  }

  @HostListener('document:keydown.escape')
  onClose(): void {
    this.isOpen = false;
    this.closed.emit();
  }
  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitted.emit(this.form.getRawValue() as CreateTaskCommand);
    this.form.reset();
    this.onClose();
  }
}
