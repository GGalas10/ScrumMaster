import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../../Core/Services/Auth.service';
import { LoginCommand } from '../../../Core/Models/UsersInterfaces';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { Router, RouterModule } from '@angular/router';
import { CustomAlertComponent } from '../../../shared/components/custom-alert/custom-alert.component';

import { ErrorModel, ErrorsNameSwitch } from '../../../shared/ErrorClass';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    TranslatePipe,
    RouterModule,
    CustomAlertComponent
],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  standalone: true,
  providers: [AuthService],
})
export class LoginComponent {
  constructor(private authService: AuthService, private router: Router) {}
  private formBuilder = inject(FormBuilder);
  title: string = '';
  errors = new ErrorModel();
  isLoginClick = false;
  form = this.formBuilder.group({
    email: ['', Validators.compose([Validators.email, Validators.required])],
    password: [
      '',
      Validators.compose([Validators.required, Validators.minLength(6)]),
    ],
  });
  onSubmit() {
    this.isLoginClick = true;
    if (!this.form.valid) {
      alert('Uzupełnij wszystkie pola');
      this.isLoginClick = false;
      return;
    }
    let command: LoginCommand = {
      email: this.form.value.email || '',
      password: this.form.value.password || '',
    };
    this.authService.LoginUser(command).subscribe({
      next: () => {
        if (this.authService.redirectUrl) {
          console.log('redirectUrl', this.authService.redirectUrl);
          this.router.navigateByUrl(this.authService.redirectUrl);
        } else {
          this.router.navigate(['/Project']);
        }
      },
      error: (err) => {
        if (err.status == 400) {
          this.errors.showOneBadRequest(err.error, 'Errors.FormInvalid');
          this.isLoginClick = false;
        } else {
          this.errors.showOneInternal();
          this.isLoginClick = false;
        }
      },
    });
  }
}
