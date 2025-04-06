import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { TokenServiceService } from '../Services/token-service.service';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../Services/Auth.service';

export const authInterceptorsInterceptor: HttpInterceptorFn = (req, next) => {
  
  const tokenService = inject(TokenServiceService);
  const authService = inject(AuthService);
  const token = tokenService.GetJwtToken();

  let clonedReq = req;
  if (token) {
    clonedReq = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` },
      withCredentials: true
    });
  } else {
    clonedReq = req.clone({ withCredentials: true });
  }
  return next(req).pipe(
    catchError((error:HttpErrorResponse) => {
      if(error.status === 401){
        return authService.Refresh().pipe(
          switchMap(() => {
            const newToken = tokenService.GetJwtToken();
            const newReq = req.clone({
              setHeaders: {Authorization : `Bearer ${newToken}`},
              withCredentials : true
            });
            return next(newReq);
          }),
          catchError(refreshError => {
            tokenService.Logout();
            return throwError(()=> refreshError);
          })
      )
      }
      return throwError(()=>error);
    })
  );
};
