import { Inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginCommand, RegisterCommand } from '../Models/UsersInterfaces';
import { TokenServiceService } from './token-service.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  headers:HttpHeaders = new HttpHeaders({"ScrumMaster":"true"});
  apiUrl = environment.identityUrl;
  constructor(private http: HttpClient,private tokenService: TokenServiceService) { }
  RegisterUser(command: RegisterCommand): Observable<string> { 
    return this.http.post<string>(`${this.apiUrl}/Register`, command,{headers:this.headers})
    .pipe(tap(response=>{
      this.tokenService.SetJwtToken(response);
    }));
  }
  LoginUser(command:LoginCommand):Observable<string>{
    return this.http.post<string>(`${this.apiUrl}/Login`,command,{headers:this.headers});
  }
  Refresh():Observable<string>{
    return this.http.post<string>(`${this.apiUrl}/Refresh`,null,{headers:this.headers, withCredentials : true});
  }
}
