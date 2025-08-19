import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { TokenService } from '../Services/token.service';
import { AuthService } from '../Services/Auth.service';
import { filter, map, take } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const tokenService = inject(TokenService);
  const authService = inject(AuthService);
  const router = inject(Router);
  authService.redirectUrl = state.url;
  return authService.isInitialized$.pipe(
    filter((initialized) => initialized),
    take(1),
    map(() => {
      return tokenService.IsLogin() ? true : router.createUrlTree(['/Login']);
    })
  );
};
