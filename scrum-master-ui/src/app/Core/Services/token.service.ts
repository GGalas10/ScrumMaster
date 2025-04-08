import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private jwt:string | null = null;
  constructor() { }
  GetJwtToken():string|null{
    return this.jwt;
  }
  SetJwtToken(token:string){
    console.log(token);
    this.jwt = token;
  }
  IsLogin():boolean{
    if(this.jwt == null)
      return false;
    return true;
  }
  Logout():void{
    this.jwt = null;
  }
}
