import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpStatusCode } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl = environment.identityUrl;
  constructor(private http: HttpClient) { }
  RegisterUser(command: RegisterCommand): Observable<HttpStatusCode> {
    return this.http.post(`${this.apiUrl}/Register`, command);
  }
}
