import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserDetails } from '../Models/UsersInterfaces';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  apiUrl = environment.identityUrl;
  constructor(private http: HttpClient) {}
  GetUserDetails(userId: string): Observable<UserDetails> {
    return this.http.get<UserDetails>(
      `${this.apiUrl}/api/User/GetById?userId=${userId}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
}
