import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  standalone: true,
  imports: [],
  selector: 'app-accept-alert',
  templateUrl: './accept-alert.component.html',
  styleUrl: './accept-alert.component.scss',
})
export class AcceptAlertComponent {
  constructor(private translate: TranslateService) {
    this.title = this.translate.instant('CustomAlert.ConfirmTitle');
    this.message = this.translate.instant('CustomAlert.ConfirmMessage');
    this.confirmText = this.translate.instant('CustomAlert.ConfirmButton');
    this.cancelText = this.translate.instant('CustomAlert.CancelButton');
  }
  @Input() title: string = '';
  @Input() message: string = '';
  @Input() confirmText: string = '';
  @Input() cancelText: string = '';
  @Output() userChoice = new EventEmitter<boolean>();
  close(result: boolean) {
    this.userChoice.emit(result);
  }
}
