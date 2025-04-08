import { Inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginCommand, RegisterCommand } from '../Models/UsersInterfaces';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  headers:HttpHeaders = new HttpHeaders({"ScrumMaster":"true"});
  apiUrl = environment.identityUrl;
  constructor(private http: HttpClient,private tokenService: TokenService) { }
  RegisterUser(command: RegisterCommand): Observable<string> { 
    return this.http.post<string>(`${this.apiUrl}/Register`, command,{headers:this.headers, withCredentials : true})
    .pipe(tap(response=>{
      this.tokenService.SetJwtToken(response);
    }));
  }
  LoginUser(command:LoginCommand):Observable<string>{
    return this.http.post<string>(`${this.apiUrl}/Login`,command,{headers:this.headers, withCredentials : true}).pipe(tap(response=>{
      this.tokenService.SetJwtToken(response);
    }));
  }
  Refresh():Observable<string>{
    return this.http.post<string>(`${this.apiUrl}/Refresh`,null,{headers:this.headers, withCredentials : true}).pipe(tap(response=>{
      console.log(response);
      this.tokenService.SetJwtToken(response);
    }));
  }
}
