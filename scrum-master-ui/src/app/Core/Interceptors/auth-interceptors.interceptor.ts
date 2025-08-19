import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { TokenService } from '../Services/token.service';
import { catchError, from, switchMap, throwError } from 'rxjs';
import { AuthService } from '../Services/Auth.service';

export const authInterceptorsInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenService = inject(TokenService);
  const authService = inject(AuthService);

  if (req.url.includes('/Refresh')) {
    return next(req);
  }
  return next(req).pipe(
    catchError((error) => {
      if (error.status === 401) {
        const retryReq = req.clone();
        return from(authService.Refresh()).pipe(
          switchMap(() => next(retryReq))
        );
      }
      return throwError(() => error);
    })
  );
};
