import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-custom-alert',
  imports: [],
  templateUrl: './custom-alert.component.html',
  styleUrl: './custom-alert.component.scss'
})
export class CustomAlertComponent {
 @Input() errors!:string[];
 @Input() title!:string;
}
