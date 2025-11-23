import { Component, inject } from '@angular/core';
import { AuthService } from '../../../Core/Services/Auth.service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { CustomAlertComponent } from '../../../shared/components/custom-alert/custom-alert.component';
import { CommonModule } from '@angular/common';
import { RegisterCommand } from '../../../Core/Models/UsersInterfaces';
import { Router } from '@angular/router';
import { ErrorModel } from '../../../shared/ErrorClass';

@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule,
    TranslatePipe,
    CustomAlertComponent,
    CommonModule,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  standalone: true,
})
export class RegisterComponent {
  private formBuilder = inject(FormBuilder);
  errorModel = new ErrorModel();
  confirmShow = false;
  passShow = false;
  constructor(private authService: AuthService, private router: Router) {}
  registerForm = this.formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    userName: ['', Validators.required],
    email: ['', Validators.compose([Validators.email, Validators.required])],
    password: [
      '',
      Validators.compose([Validators.minLength(10), Validators.required]),
    ],
    confirmPassword: [
      '',
      Validators.compose([Validators.minLength(10), Validators.required]),
    ],
  });
  GoToLogin() {
    this.router.navigate(['/Login']);
  }
  onSpace(event: KeyboardEvent): void {
    if (event.code === 'Space' || event.key === ' ') {
      event.preventDefault();
    }
  }
  RegisterUser() {
    if (!this.registerForm.valid) {
      this.errorModel.showMoreErrors(this.ValidForm(), 'Errors.FormInvalid');
      return;
    }
    if (
      this.registerForm.value.confirmPassword !=
      this.registerForm.value.password
    ) {
      this.errorModel.showOneBadRequest(
        'Passwords_Incorrect',
        'Errors.FormInvalid'
      );
      console.log(this.errorModel);
      return;
    } else {
      this.authService.RegisterUser(this.GetModelFromForm()).subscribe({
        next: (response) => {
          this.router.navigate(['/']);
        },
        error: (err) => {
          if (err.status == 400)
            this.errorModel.showOneBadRequest(err.error, 'Errors.FormInvalid');
          this.errorModel.showOneInternal();
        },
      });
    }
  }
  GetModelFromForm(): RegisterCommand {
    return {
      firstName: this.registerForm.value.firstName ?? '',
      lastName: this.registerForm.value.lastName ?? '',
      userName: this.registerForm.value.userName ?? '',
      email: this.registerForm.value.email ?? '',
      password: this.registerForm.value.password ?? '',
      confirmPassword: this.registerForm.value.confirmPassword ?? '',
    };
  }
  ValidForm(): string[] {
    let errors: string[] = [];
    if (this.registerForm.controls['firstName'].invalid)
      errors.push('Errors.FirstNameRequired');

    if (this.registerForm.controls['lastName'].invalid)
      errors.push('Errors.LastNameRequired');

    if (this.registerForm.controls['userName'].invalid)
      errors.push('Errors.UserNameRequired');

    if (this.registerForm.controls['email'].invalid)
      errors.push('Errors.EmaiRequired');

    if (this.registerForm.controls['password'].errors?.['required'])
      errors.push('Errors.PasswordRequired');

    if (this.registerForm.controls['confirmPassword'].errors?.['required'])
      errors.push('Errors.ConfirmPasswordRequired');

    if (this.registerForm.controls['confirmPassword'].errors?.['minlength'])
      errors.push('Errors.Passwords_Incorrect');

    if (this.registerForm.controls['password'].errors?.['minlength'])
      errors.push('Errors.PasswordMinLength');
    return errors;
  }
}
