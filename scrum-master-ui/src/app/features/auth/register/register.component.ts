import { Component, inject } from '@angular/core';
import { AuthService } from '../../../Core/Services/Auth.service';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-register',
  imports: [TranslatePipe],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  standalone: true
})
export class RegisterComponent {
  constructor(private authService:AuthService){}
  private formBuilder = inject(FormBuilder);
  registerForm = this.formBuilder.group({
    firstName : ['',Validators.required,],
    lastName : ['',Validators.required],
    email: ['',Validators.compose([Validators.email, Validators.required])],
    password: ['',Validators.compose([Validators.minLength(6), Validators.required])],
    confirmPassword: ['',Validators.compose([Validators.minLength(6), Validators.required])],
  })
}
