import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../Core/Services/Auth.service';
import { LoginCommand } from '../../../Core/Models/UsersInterfaces';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  standalone: true,
  providers: [AuthService]
})
export class LoginComponent {
  constructor(private authService:AuthService){}
  onSubmit(){
    let command:LoginCommand = {email:"Test",password:"Test"};
    this.authService.LoginUser(command).subscribe({
      next: result => console.log(result),
      error: err => {
        if(err.error.includes("Wrong_Credentials")){
          alert("Podano błedne dane logowania")
        }
      },
    });
  }
}
