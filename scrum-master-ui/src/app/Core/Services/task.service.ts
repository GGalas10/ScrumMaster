import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import { TaskStatuses } from '../Models/TaskInterfaces';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  headers: HttpHeaders = new HttpHeaders({ ScrumMaster: 'true' });
  apiUrl = environment.taskUrl;
  constructor(private http: HttpClient) {}
  GetTaskStatuses(): Observable<TaskStatuses[]> {
    return this.http.get<TaskStatuses[]>(`${this.apiUrl}/GetTasksStatuses`, {
      headers: this.headers,
      withCredentials: true,
    });
  }
}
