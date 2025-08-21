import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import { SprintList } from '../Models/SprintInterfaces';

@Injectable({
  providedIn: 'root',
})
export class BoardService {
  headers: HttpHeaders = new HttpHeaders({ ScrumMaster: 'true' });
  apiUrl = environment.identityUrl;
  sprintUrl = environment.sprintUrl;
  constructor(private http: HttpClient) {}
  GetBoardInfo(): Observable<string> {
    return this.http.get<string>(`${this.apiUrl}/BoardInfo`, {
      headers: this.headers,
      withCredentials: true,
    });
  }
  GetSprintsForProject(projectId: string): Observable<SprintList[]> {
    return this.http.get<SprintList[]>(
      `${this.sprintUrl}/GetSprintsByProjectId?projectId=${projectId}`,
      {
        headers: this.headers,
        withCredentials: true,
      }
    );
  }
}
