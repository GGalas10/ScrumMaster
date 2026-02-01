import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  AddMemeberCommand,
  CreateProject,
  ProjectMember,
  UserProject,
} from '../Models/ProjectInterfaces';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  projectUrl = environment.projectUrl;
  constructor(private http: HttpClient) {}
  GetUsersProject(): Observable<UserProject[]> {
    return this.http.get<UserProject[]>(`${this.projectUrl}/GetUserProjects`, {
      headers: environment.headers,
      withCredentials: true,
    });
  }
  CreateProject(command: CreateProject): Observable<string> {
    return this.http.post<string>(`${this.projectUrl}/CreateProject`, command, {
      headers: environment.headers,
      withCredentials: true,
    });
  }
  GetProjectMembers(projectId: string): Observable<ProjectMember[]> {
    return this.http.get<ProjectMember[]>(
      `${this.projectUrl}/GetAllProjectMembers?projectId=${projectId}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
  CanManageMembers(projectId: string): Observable<boolean> {
    return this.http.get<boolean>(
      `${this.projectUrl}/CanManageMembers?projectId=${projectId}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
  AddMemeberToProject(command: AddMemeberCommand): Observable<void> {
    return this.http.post<void>(`${this.projectUrl}/AddUserAccess`, command, {
      headers: environment.headers,
      withCredentials: true,
    });
  }
}
