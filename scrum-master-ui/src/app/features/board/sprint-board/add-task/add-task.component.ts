import { trigger, transition, style, animate } from '@angular/animations';
import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
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
  imports: [ReactiveFormsModule, TranslatePipe, CommonModule],
  templateUrl: './add-task.component.html',
  styleUrl: './add-task.component.scss',
  animations: [
    trigger('slideDown', [
      transition(':enter', [
        style({ transform: 'translateY(-100%)', opacity: 0 }),
        animate(
          '300ms ease-out',
          style({ transform: 'translateY(0)', opacity: 1 })
        ),
      ]),
      transition(':leave', [
        animate(
          '250ms ease-in',
          style({ transform: 'translateY(-100%)', opacity: 0 })
        ),
      ]),
    ]),
  ],
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
