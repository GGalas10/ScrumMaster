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
  projectUrl = environment.projectUrl;
  sprintUrl = environment.sprintUrl;
  constructor(private http: HttpClient) {}
  GetBoardInfo(projectId: string): Observable<string> {
    return this.http.get<string>(
      `${this.projectUrl}/GetBoardInfo?projectId=${projectId}`,
      {
        headers: this.headers,
        withCredentials: true,
      }
    );
  }
}
