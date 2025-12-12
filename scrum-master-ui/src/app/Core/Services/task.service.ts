import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import {
  CreateTaskCommand,
  TaskDTO,
  TaskListDTO,
  TaskStatuses,
} from '../Models/TaskInterfaces';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  constructor(private http: HttpClient) {}
  GetTaskStatuses(): Observable<TaskStatuses[]> {
    return this.http.get<TaskStatuses[]>(
      `${environment.taskUrl}/GetTasksStatuses`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
  CreateTask(command: CreateTaskCommand): Observable<string> {
    return this.http.post<string>(
      `${environment.taskUrl}/CreateTask`,
      command,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
  GetAllSprintTasks(sprintId: string): Observable<TaskListDTO[]> {
    return this.http.get<TaskListDTO[]>(
      `${environment.taskUrl}/GetAllSprintTasks?sprintId=${sprintId}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
  GetTaskDetails(taskId: string): Observable<TaskDTO> {
    return this.http.get<TaskDTO>(
      `${environment.taskUrl}/GetTaskById?taskId=${taskId}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
}
