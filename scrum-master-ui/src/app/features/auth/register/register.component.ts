import { Component, inject } from '@angular/core';
import { AuthService } from '../../../Core/Services/Auth.service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { CustomAlertComponent } from "../../../shared/components/custom-alert/custom-alert.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, TranslatePipe, CustomAlertComponent,CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  standalone: true
})
export class RegisterComponent {
  private formBuilder = inject(FormBuilder);
  errors:string[]=[];
  title:string = "";
  showModal = false;
  constructor(private authService:AuthService){}
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
      if(this.registerForm.controls['lastName'].invalid)
        errors.push("Errors.LastNameRequired")
      if(this.registerForm.controls['email'].invalid)
        errors.push("Errors.EmaiRequired")
      if(this.registerForm.controls['password'].invalid)
        errors.push("Errors.PasswordRequired")
      if(this.registerForm.controls['confirmPassword'].invalid)
        errors.push("Errors.ConfirmPasswordRequired")
      this.ShowErrorModal(errors,"Errors.FormInvalid")
    }
  }
  ShowErrorModal(errors:string[],title:string){
    this.errors = errors;
    this.title = title;
    this.showModal = true;
  }
}
