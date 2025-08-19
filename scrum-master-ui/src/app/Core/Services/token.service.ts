import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private isAuthenticated = false;
  private userName = '';
  constructor() {}
  GetUserInfos(): string {
    return this.userName;
  }
  Login(userName: string): void {
    this.userName = userName;
    this.isAuthenticated = true;
  }
  IsLogin(): boolean {
    return this.isAuthenticated;
  }
  Logout(): void {
    this.isAuthenticated = false;
    this.userName = '';
  }
}
