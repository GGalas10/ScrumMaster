import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserDetails, UserListDTO } from '../Models/UsersInterfaces';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient) {}
  GetUserDetails(userId: string): Observable<UserDetails> {
    return this.http.get<UserDetails>(
      `${environment.identityUrl}/api/User/GetById?userId=${userId}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
  FindUser(filter: string): Observable<UserListDTO[]> {
    return this.http.get<UserListDTO[]>(
      `${environment.identityUrl}/api/User/FindUsers?filter=${filter}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
}
