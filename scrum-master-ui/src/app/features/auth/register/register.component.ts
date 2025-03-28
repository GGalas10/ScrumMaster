import { Component, inject } from '@angular/core';
import { AuthService } from '../../../Core/Services/Auth.service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule,TranslatePipe],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  standalone: true
})
export class RegisterComponent {
  constructor(private authService:AuthService){}
  private formBuilder = inject(FormBuilder);
  registerForm = this.formBuilder.group({
    firstName : ['',Validators.required],
    lastName : ['',Validators.required],
    email: ['',Validators.compose([Validators.email, Validators.required])],
    password: ['',Validators.compose([Validators.minLength(6), Validators.required])],
    confirmPassword: ['',Validators.compose([Validators.minLength(6), Validators.required])],
  })
  GoToLogin(){
    location.href = '/Login';
  }
  RegisterUser(){
    if(!this.registerForm.valid){
      let errors:string[] = [];
      if(this.registerForm.controls['firstName'].invalid)
        errors.push("Errors.FirstNameRequired")
    }
  }
}
