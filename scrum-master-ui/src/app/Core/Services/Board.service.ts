import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import { SprintList } from '../Models/SprintInterfaces';
import { UserProject } from '../Models/ProjectInterfaces';

@Injectable({
  providedIn: 'root',
})
export class BoardService {
  headers: HttpHeaders = new HttpHeaders({ ScrumMaster: 'true' });
  apiUrl = environment.identityUrl;
  sprintUrl = environment.sprintUrl;
  projectUrl = environment.projectUrl;
  constructor(private http: HttpClient) {}
  GetBoardInfo(): Observable<string> {
    return this.http.get<string>(`${this.apiUrl}/BoardInfo`, {
      headers: this.headers,
      withCredentials: true,
    });
  }
  GetUsersProject(): Observable<UserProject[]> {
    return this.http.get<UserProject[]>(`${this.projectUrl}/GetUserProjects`, {
      headers: this.headers,
      withCredentials: true,
    });
  }
}
