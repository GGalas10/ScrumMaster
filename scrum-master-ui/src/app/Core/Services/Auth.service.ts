import { Inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginCommand, RegisterCommand } from '../Models/UsersInterfaces';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  headers: HttpHeaders = new HttpHeaders({ ScrumMaster: 'true' });
  apiUrl = environment.identityUrl;
  redirectUrl = '';
  private _initialized$ = new BehaviorSubject(false);
  isInitialized$ = this._initialized$.asObservable();
  constructor(private http: HttpClient, private tokenService: TokenService) {}
  RegisterUser(command: RegisterCommand): Observable<string> {
    return this.http
      .post<string>(`${this.apiUrl}/Register`, command, {
        headers: this.headers,
        withCredentials: true,
      })
      .pipe(
        tap((result) => {
          this.tokenService.Login(result);
        })
      );
  }
  LoginUser(command: LoginCommand): Observable<string> {
    return this.http
      .post<string>(`${this.apiUrl}/Login`, command, {
        headers: this.headers,
        withCredentials: true,
      })
      .pipe(
        tap((result) => {
          this.tokenService.Login(result);
        })
      );
  }
  Refresh(): Promise<void> {
    return new Promise<void>(async (resolve) => {
      await this.http
        .post<string>(`${this.apiUrl}/Refresh`, {}, { withCredentials: true })
        .subscribe({
          next: (result) => {
            this.tokenService.Login(result);
            resolve();
          },
          error: (err) => {
            this.tokenService.Logout();
            resolve();
          },
        });
    }).finally(() => {
      this._initialized$.next(true);
    });
  }
  Logout(): Observable<void> {
    this.tokenService.Logout();
    return this.http.get<void>(`${this.apiUrl}/Logout`, {
      headers: this.headers,
    });
  }
  Ping(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/HealthCheck`, {
      headers: this.headers,
      withCredentials: true,
    });
  }
}
