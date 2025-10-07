import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { CreateProject } from '../../Core/Models/ProjectInterfaces';

@Component({
  selector: 'app-new-project-modal',
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './new-project-modal.component.html',
  styleUrl: './new-project-modal.component.scss',
})
export class NewProjectModalComponent implements OnInit, OnDestroy {
  @Input() open = false;
  @Output() openChange = new EventEmitter<boolean>();
  @Output() create = new EventEmitter<CreateProject>();

  @ViewChild('sheetRoot') sheetRoot!: ElementRef<HTMLElement>;
  @ViewChild('firstField') firstField!: ElementRef<HTMLInputElement>;

  name = '';
  key = '';
  description = '';

  ngOnInit() {
    if (this.open) this.afterOpen();
  }
  ngOnDestroy() {
    document.body.classList.remove('no-scroll');
  }

  ngOnChanges() {
    if (this.open) this.afterOpen();
    else document.body.classList.remove('no-scroll');
  }

  private afterOpen() {
    document.body.classList.add('no-scroll');
    this.name = '';
    this.description = '';
    queueMicrotask(() => {
      this.sheetRoot?.nativeElement.focus();
      this.firstField?.nativeElement.focus();
    });
  }

  openSheet() {
    if (!this.open) {
      this.open = true;
      this.openChange.emit(true);
      this.afterOpen();
    }
  }
  close() {
    if (this.open) {
      this.open = false;
      this.openChange.emit(false);
      document.body.classList.remove('no-scroll');
    }
  }
  onBackdrop() {
    this.close();
  }
  submit() {
    if (!this.name?.trim() || !this.description.trim()) return;
    const payload: CreateProject = {
      projectName: this.name.trim(),
      projectDescription: this.description.trim(),
    };
    this.create.emit(payload);
    this.close();
    this.name = '';
    this.description = '';
  }
}
