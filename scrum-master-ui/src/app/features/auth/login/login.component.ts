import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../../Core/Services/Auth.service';
import { LoginCommand } from '../../../Core/Models/UsersInterfaces';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { Router, RouterModule } from '@angular/router';
import { CustomAlertComponent } from '../../../shared/components/custom-alert/custom-alert.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    TranslatePipe,
    RouterModule,
    CustomAlertComponent,
    CommonModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  standalone: true,
  providers: [AuthService],
})
export class LoginComponent {
  constructor(private authService: AuthService, private router: Router) {}
  private formBuilder = inject(FormBuilder);
  title: string = 'Errors.FormInvalid';
  errors: string[] = [];
  isLoginClick = false;
  showModal = false;
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
      return;
    }
    let command: LoginCommand = {
      email: this.form.value.email || '',
      password: this.form.value.password || '',
    };
    this.authService.LoginUser(command).subscribe({
      next: () => {
        console.log(this.authService.redirectUrl);
        if (this.authService.redirectUrl) {
          console.log('redirectUrl', this.authService.redirectUrl);
          this.router.navigateByUrl(this.authService.redirectUrl);
        } else {
          this.router.navigate(['/Board']);
        }
      },
      error: (err) => {
        this.errors = [];
        if (
          typeof err.error === 'string' &&
          err.error.includes('Wrong_Credentials')
        ) {
          this.errors.push('Errors.WrongCredentials');
          this.isLoginClick = false;
        } else {
          this.errors.push('Errors.SomethingWrong');
          this.isLoginClick = false;
        }
        this.showModal = true;
      },
    });
  }
}
