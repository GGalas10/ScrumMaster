import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BoardService {
  headers: HttpHeaders = new HttpHeaders({ ScrumMaster: 'true' });
  apiUrl = environment.identityUrl;
  constructor(private http: HttpClient) {}
  GetBoardInfo(): Observable<string> {
    return this.http.get<string>(`${this.apiUrl}/BoardInfo`, {
      headers: this.headers,
      withCredentials: true,
    });
  }
}
