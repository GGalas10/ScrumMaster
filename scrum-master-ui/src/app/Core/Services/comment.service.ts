import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Comment, CreateCommentCommand } from '../Models/TaskCommentInterfaces';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class CommentService {
  constructor(private http: HttpClient) {}
  GetCommentByTaskId(taskId: string): Observable<Comment[]> {
    return this.http.get<Comment[]>(
      `${environment.taskUrl}/Comment/GetTaskComment?taskId=${taskId}`,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }

  SendComment(command: CreateCommentCommand): Observable<void> {
    return this.http.post<void>(
      `${environment.taskUrl}/Comment/SendComment`,
      command,
      {
        headers: environment.headers,
        withCredentials: true,
      }
    );
  }
}
