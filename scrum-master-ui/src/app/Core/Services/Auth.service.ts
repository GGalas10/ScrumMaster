import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpStatusCode } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginCommand, RegisterCommand } from '../Models/UsersInterfaces';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl = environment.identityUrl;
  constructor(private http: HttpClient) { }
  RegisterUser(command: RegisterCommand): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/Register`, command);
  }
  LoginUser(command:LoginCommand):Observable<string>{
    return this.http.post<string>(`${this.apiUrl}/Login`,command);
  }
}
