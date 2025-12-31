import {
  Component,
  DestroyRef,
  EventEmitter,
  inject,
  Input,
  input,
  Output,
} from '@angular/core';
import { UserService } from '../../../Core/Services/user.service';
import { UserListDTO } from '../../../Core/Models/UsersInterfaces';
import { TranslatePipe } from '@ngx-translate/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  map,
  debounceTime,
  distinctUntilChanged,
  filter,
  exhaustMap,
  finalize,
  catchError,
  of,
} from 'rxjs';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
  AddMemeberCommand,
  ProjectRoleEnum,
} from '../../../Core/Models/ProjectInterfaces';
import { QueryParameterService } from '../../../shared/query-parameter.service';
import { CustomAlertComponent } from '../../../shared/components/custom-alert/custom-alert.component';
import { ErrorModel } from '../../../shared/ErrorClass';

@Component({
  selector: 'app-mange-members',
  imports: [
    TranslatePipe,
    CommonModule,
    ReactiveFormsModule,
    CustomAlertComponent,
  ],
  templateUrl: './mange-members.component.html',
  styleUrl: './mange-members.component.scss',
})
export class MangeMembersComponent {
  private destroyRef = inject(DestroyRef);
  roleOptions = Object.values(ProjectRoleEnum).filter(
    (v) => typeof v === 'number'
  ) as number[];
  errorModel: ErrorModel = new ErrorModel();
  selectedRole = new FormControl(0, { nonNullable: true });
  userList: UserListDTO[] = [];
  query = new FormControl('', { nonNullable: true });
  isLoading = false;
  selectedUser: UserListDTO | null = null;
  @Input() isOpen = false;
  @Output() closeModal = new EventEmitter<void>();
  ProjectRoleEnum = ProjectRoleEnum;
  constructor(
    private userService: UserService,
    private queryParameter: QueryParameterService
  ) {
    this.errorModel.hide();
    this.query.valueChanges
      .pipe(
        map((v) => v.trim()),
        debounceTime(300),
        distinctUntilChanged(),
        filter((v) => v.length > 0),
        exhaustMap((queryMap) => {
          this.isLoading = true;

          return userService.FindUser(queryMap).pipe(
            finalize(() => (this.isLoading = false)),
            catchError((err) => {
              console.error(err);
              return of([] as UserListDTO[]);
            })
          );
        }),

        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe((res) => {
        this.userList = res;
      });
  }
  onOverlayClick(): void {
    this.closeModal.emit();
    this.selectedRole.setValue(0);
    this.selectedUser = null;
    this.errorModel.hide();
    this.query.setValue('');
    this.userList = [];
  }

  onModalClick(event: MouseEvent): void {
    event.stopPropagation();
  }
  SelectOne(user: UserListDTO) {
    if (user == this.selectedUser) {
      this.selectedUser = null;
      return;
    }
    this.selectedUser = user;
  }
  Submit() {
    if (this.selectedRole.value === 0 || this.selectedUser == null) {
      this.errorModel.showOneBadRequest(
        'Errors.RoleRequired',
        'Errors.FormInvalid'
      );
      return;
    }
    var command: AddMemeberCommand = {
      projectId: this.queryParameter.getQueryParam('id'),
      userId: this.selectedUser?.id ?? '',
      userRole: this.selectedRole.value,
    };
    console.log(command);
  }
}
