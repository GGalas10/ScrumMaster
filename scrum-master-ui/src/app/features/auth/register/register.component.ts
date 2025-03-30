import { Component, inject } from '@angular/core';
import { AuthService } from '../../../Core/Services/Auth.service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { CustomAlertComponent } from "../../../shared/components/custom-alert/custom-alert.component";
import { CommonModule } from '@angular/common';
import { RegisterCommand } from '../../../Core/Models/UsersInterfaces';

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
    userName : ['',Validators.required],
    email: ['',Validators.compose([Validators.email, Validators.required])],
    password: ['',Validators.compose([Validators.minLength(6), Validators.required])],
    confirmPassword: ['',Validators.compose([Validators.minLength(6), Validators.required])],
  })
  GoToLogin(){
    location.href = '/Login';
  }
  RegisterUser(){
    let errors:string[] = [];
    if(!this.registerForm.valid){
      if(this.registerForm.controls['firstName'].invalid)
        errors.push("Errors.FirstNameRequired")

      if(this.registerForm.controls['lastName'].invalid)
        errors.push("Errors.LastNameRequired")

      if(this.registerForm.controls['userName'].invalid)
        errors.push("Errors.UserNameRequired")

      if(this.registerForm.controls['email'].invalid)
        errors.push("Errors.EmaiRequired")

      if(this.registerForm.controls['password'].errors?.['required'])
        errors.push("Errors.PasswordRequired")

      if(this.registerForm.controls['confirmPassword'].errors?.['required'])
        errors.push("Errors.ConfirmPasswordRequired")

      if(this.registerForm.controls['password'].errors?.['minlength'])
        errors.push("Errors.PasswordMinLength")

      this.ShowErrorModal(errors,"Errors.FormInvalid");
      return;
    }
    if(this.registerForm.value.confirmPassword == this.registerForm.value.password){
      errors.push('Errors.IncorrectPasswords')
      this.ShowErrorModal(errors,"Errors.FormInvalid");
      return;
    }else{
      this.authService.RegisterUser(this.GetModelFromForm()).subscribe({
        next : (response) => {
          console.log(response);
        },
        error : (err) => {
          let errors:string[] = [];
          if(err.error == "Command_Is_Null")
            errors.push('Errors.SomethingWrong')

          if(err.error == "Email_Cannot_Be_Null")
            errors.push('Errors.EmaiRequired')

          if(err.error == "Password_Cannot_Be_Null")
            errors.push('Errors.PasswordRequired')

          if(err.error == "FirstName_Cannot_Be_Null")
            errors.push('Errors.FirstNameRequired')

          if(err.error == "LastName_Cannot_Be_Null")
            errors.push('Errors.LastNameRequired')

          if(err.error == "LastName_Cannot_Be_Null")
            errors.push('Errors.LastNameRequired')

          if(err.error == "UserName_Cannot_Be_Null")
            errors.push('Errors.PasswordMinLength')

          if(err.error == "Passwords_Incorrect")
            errors.push('Errors.IncorrectPasswords')

          if(err.error == "User_Email_Already_Exist")
            errors.push('Errors.EmailAlreadyExist')

          if(err.error == "User_Name_Already_Exist")
            errors.push('Errors.UserNameAlreadyExist')

          this.ShowErrorModal(errors,"Errors.FormInvalid");
        }
      })
    }
}
  ShowErrorModal(errors:string[],title:string){
    this.errors = errors;
    this.title = title;
    this.showModal = true;
  }
  GetModelFromForm():RegisterCommand{
    return {
      firstName : this.registerForm.value.firstName ?? "",
      lastName : this.registerForm.value.lastName ?? "",
      userName : this.registerForm.value.userName ?? "",
      email : this.registerForm.value.email ?? "",
      password : this.registerForm.value.password ?? "",
      confirmPassword : this.registerForm.value.confirmPassword ?? ""
    };
  }
}
