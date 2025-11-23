import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { ErrorModel } from '../../ErrorClass';

@Component({
  selector: 'app-custom-alert',
  imports: [TranslatePipe],
  templateUrl: './custom-alert.component.html',
  styleUrl: './custom-alert.component.scss',
  standalone: true,
})
export class CustomAlertComponent {
  @Input() errorModel!: ErrorModel;
  @Output() closeEvent = new EventEmitter<void>();

  CloseModal() {
    this.closeEvent.emit();
  }
}
