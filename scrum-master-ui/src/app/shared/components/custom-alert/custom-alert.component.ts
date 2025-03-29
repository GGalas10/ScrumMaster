import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-custom-alert',
  imports: [TranslatePipe],
  templateUrl: './custom-alert.component.html',
  styleUrl: './custom-alert.component.scss',
  standalone : true
})
export class CustomAlertComponent {
 @Input() errors!:string[];
 @Input() title!:string;
 @Output() closeEvent = new EventEmitter<void>();

 CloseModal(){
  this.closeEvent.emit();
 }
}
