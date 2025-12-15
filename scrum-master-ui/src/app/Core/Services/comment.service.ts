import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Comment } from '../Models/TaskCommentInterfaces';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class CommentService {
  constructor(private http: HttpClient) {}
  GetCommentByTaskId(taskId: string): Observable<Comment[]> {
    return this.http.get<Comment[]>(
      `${environment.taskUrl}/Comments/GetTaskComment?taskId=${taskId}`
    );
  }
}
