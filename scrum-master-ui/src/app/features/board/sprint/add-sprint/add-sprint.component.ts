import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CreateSprintCommand } from '../../../../Core/Models/SprintInterfaces';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';
import { animate, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-add-sprint',
  imports: [ReactiveFormsModule, CommonModule, TranslatePipe],
  templateUrl: './add-sprint.component.html',
  styleUrl: './add-sprint.component.scss',
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
export class AddSprintComponent {
  @Input() isOpen = false;
  @Input() data: CreateSprintCommand | null = null;

  @Output() submitted = new EventEmitter<CreateSprintCommand>();
  @Output() closed = new EventEmitter<void>();
  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group(
      {
        name: ['', Validators.required],
        startDate: [this.fb.control<Date>, Validators.required],
        endDate: [this.fb.control<Date>, Validators.required],
      },
      { validators: this.dateRangeValidator }
    );
  }
  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitted.emit(this.form.getRawValue() as CreateSprintCommand);
    this.form.reset();
    this.onClose();
  }

  onClose(): void {
    this.isOpen = false;
    this.closed.emit();
  }
  dateRangeValidator(form: FormGroup) {
    const start: Date | null = form.get('startDate')?.value;
    const end: Date | null = form.get('endDate')?.value;

    if (!start || !end) return null;

    return start <= end ? null : { dateRangeInvalid: true };
  }
}
