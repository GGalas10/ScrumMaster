import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../../Core/Services/Auth.service';
import { LoginCommand } from '../../../Core/Models/UsersInterfaces';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule,TranslatePipe],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  standalone: true,
  providers: [AuthService],
})
export class LoginComponent {
  constructor(private authService:AuthService, private router:Router){}
  private formBuilder = inject(FormBuilder);
  form = this.formBuilder.group({
    email: ['',Validators.compose([Validators.email, Validators.required])],
    password: ['',Validators.compose([Validators.required,Validators.minLength(6)])]
  });
  onSubmit(){
    if(!this.form.valid){
      alert("Uzupełnij wszystkie pola");
      return;
    }
    let command:LoginCommand = {email:this.form.value.email || "",password: this.form.value.password || ""};
    this.authService.LoginUser(command).subscribe({
      next: result => this.router.navigate(['/Board']),
      error: err => {
        if(err.error.includes("Wrong_Credentials")){
          alert("Podano błedne dane logowania")
        }
      },
    });
  }
  GoToRegister(){
    this.router.navigate(['/Register']);
  }
}
