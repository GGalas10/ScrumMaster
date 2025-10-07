import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CreateProject, UserProject } from '../Models/ProjectInterfaces';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  headers: HttpHeaders = new HttpHeaders({ ScrumMaster: 'true' });
  projectUrl = environment.projectUrl;
  constructor(private http: HttpClient) {}
  GetUsersProject(): Observable<UserProject[]> {
    return this.http.get<UserProject[]>(`${this.projectUrl}/GetUserProjects`, {
      headers: this.headers,
      withCredentials: true,
    });
  }
  CreateProject(command: CreateProject): Observable<string> {
    return this.http.post<string>(`${this.projectUrl}/CreateProject`, command, {
      headers: this.headers,
      withCredentials: true,
    });
  }
}
