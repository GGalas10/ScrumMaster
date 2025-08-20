import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-add-btn',
  imports: [CommonModule],
  templateUrl: './add-btn.component.html',
  styleUrl: './add-btn.component.scss',
})
export class AddBtnComponent {
  @Input() tooltip = 'Dodaj';
  @Input() size: number = 25;
  @Output() add = new EventEmitter<void>();
  onClick() {
    this.add.emit();
  }
}
