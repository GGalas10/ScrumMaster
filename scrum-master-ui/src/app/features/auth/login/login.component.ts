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
export class LoginComponent implements OnInit {
  constructor(private authService:AuthService){}
  ngOnInit(): void {
    console.log("Test");
  }
  onSubmit(){
    let command:LoginCommand = {email:"Test",password:"Test"};
    console.log(command);
    this.authService.LoginUser(command).subscribe({
      next: result => console.log(result),
      error: err => console.log(err),
    });
  }
}
