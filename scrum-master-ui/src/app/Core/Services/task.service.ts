import { HttpClient } from '@angular/common/http';
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
      `${environment.taskUrl}/Task/GetTasksStatuses`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
  CreateTask(command: CreateTaskCommand): Observable<string> {
    return this.http.post<string>(
      `${environment.taskUrl}/Task/CreateTask`,
      command,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
  GetAllSprintTasks(sprintId: string): Observable<TaskListDTO[]> {
    return this.http.get<TaskListDTO[]>(
      `${environment.taskUrl}/Task/GetAllSprintTasks?sprintId=${sprintId}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
  GetTaskDetails(taskId: string): Observable<TaskDTO> {
    return this.http.get<TaskDTO>(
      `${environment.taskUrl}/Task/GetTaskById?taskId=${taskId}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
  UpdateTaskStatus(taskId: string, newStatus: number): Observable<void> {
    return this.http.put<void>(
      `${environment.taskUrl}/Task/UpdateTaskStatus`,
      { taskId: taskId, status: newStatus },
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
  DeleteTask(taskId: string): Observable<void> {
    return this.http.delete<void>(
      `${environment.taskUrl}/Task/DeleteTask?taskId=${taskId}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
}
