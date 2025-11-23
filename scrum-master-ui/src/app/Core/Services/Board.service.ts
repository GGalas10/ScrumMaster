import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import { SprintList } from '../Models/SprintInterfaces';
import { UserProject } from '../Models/ProjectInterfaces';
import { BoardDto } from '../Models/BoardInterfaces';

@Injectable({
  providedIn: 'root',
})
export class BoardService {
  projectUrl = environment.projectUrl;
  sprintUrl = environment.sprintUrl;
  constructor(private http: HttpClient) {}
  GetBoardInfo(projectId: string): Observable<BoardDto> {
    return this.http.get<BoardDto>(
      `${this.projectUrl}/GetBoardInfo?projectId=${projectId}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
}
