import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenServiceService {
  private jwt:string | null = null;
  constructor() { }
  GetJwtToken():string|null{
    return this.jwt;
  }
  SetJwtToken(token:string){
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
