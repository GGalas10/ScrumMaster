import { CanActivateFn, Router } from '@angular/router';
import { TokenService } from '../Services/token.service';
import { AuthService } from '../Services/Auth.service';
import { inject } from '@angular/core';
import { filter, map, take } from 'rxjs';

export const checkLoginGuard: CanActivateFn = (route, state) => {
  const tokenService = inject(TokenService);
  const authService = inject(AuthService);
  const router = inject(Router);
  authService.redirectUrl = state.url;
  return authService.isInitialized$.pipe(
    filter((initialized) => initialized),
    take(1),
    map(() => {
      return tokenService.IsLogin() ? router.createUrlTree(['/Board']) : true;
    })
  );
};
