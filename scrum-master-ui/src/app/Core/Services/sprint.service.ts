import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import { SprintDTO } from '../Models/SprintInterfaces';

@Injectable({
  providedIn: 'root',
})
export class SprintService {
  constructor(private http: HttpClient) {}
  GetProjectSprints(projectId: string): Observable<SprintDTO[]> {
    return this.http.get<SprintDTO[]>(
      `${environment.sprintUrl}/GetSprintsByProjectId?projectId=${projectId}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
}
