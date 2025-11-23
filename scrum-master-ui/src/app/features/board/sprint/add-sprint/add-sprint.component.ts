import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CreateSprintCommand } from '../../../../Core/Models/SprintInterfaces';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-sprint',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './add-sprint.component.html',
  styleUrl: './add-sprint.component.scss',
})
export class AddSprintComponent {
  @Input() isOpen = false;
  @Input() data: CreateSprintCommand | null = null;

  @Output() submitted = new EventEmitter<CreateSprintCommand>();
  @Output() closed = new EventEmitter<void>();
  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      startDate: this.fb.control<Date | null>(null),
      endDate: this.fb.control<Date | null>(null),
    });
  }
  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitted.emit(this.form.getRawValue() as CreateSprintCommand);
    this.onClose();
  }

  onClose(): void {
    this.isOpen = false; // tylko lokalnie, rodzic i tak steruje flagą
    this.closed.emit();
  }
}
